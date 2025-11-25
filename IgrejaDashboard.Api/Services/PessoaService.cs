using IgrejaDashboard.Api.Data;
using IgrejaDashboard.Api.DTOs;
using IgrejaDashboard.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace IgrejaDashboard.Api.Services
{
    public class PessoaService
    {
        private readonly AppDbContext _context;

        public PessoaService(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/pessoas
        public async Task<IEnumerable<PessoaDTO>> GetAllAsync(string? search)
        {
            var query = _context.Pessoas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Nome.Contains(search) ||
                    p.Email.Contains(search));
            }

            return await query
                .Select(p => new PessoaDTO
                {
                    Codigo = p.Codigo,
                    Nome = p.Nome,
                    Email = p.Email,
                    Sexo = p.Sexo,
                    Status = p.Status.ToString()
                })
                .ToListAsync();
        }


        // GET /api/pessoas/dashboard
        public async Task<object> GetDashboardAsync()
        {
            var total = await _context.Pessoas.CountAsync();
            // var total = await _context.Pessoas.CountAsync(p => p.Status == SituacaoPessoa.Membro);
            var masculinos = await _context.Pessoas.CountAsync(p => p.Sexo == "Masculino" );
            var femininos = await _context.Pessoas.CountAsync(p => p.Sexo == "Feminino");

            return new { total, masculinos, femininos };
        }


        // POST /api/pessoas
        public async Task<PessoaDTO> AddAsync(PessoaCreateDTO dto)
        {
            Enum.TryParse<SituacaoPessoa>(dto.Status, true, out var statusEnum);
            var pessoa = new Pessoa
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Sexo = dto.Sexo,
                Status = statusEnum
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            return new PessoaDTO
            {
                Codigo = pessoa.Codigo,
                Nome = pessoa.Nome,
                Email = pessoa.Email,
                Sexo = pessoa.Sexo,
                Status = pessoa.Status.ToString()
            };
        }


        // PUT /api/pessoas/{id}
        public async Task<bool> UpdateAsync(int id, PessoaUpdateDTO dto)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if(pessoa == null){
                return false;
            }

            if (!string.IsNullOrWhiteSpace(dto.Nome))
                pessoa.Nome = dto.Nome;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                pessoa.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Sexo))
                pessoa.Sexo = dto.Sexo;

            if (!string.IsNullOrWhiteSpace(dto.Status))
            {
                if (Enum.TryParse<SituacaoPessoa>(dto.Status, true, out var situacao))
                    pessoa.Status = situacao;
            }

            await _context.SaveChangesAsync();
            return true;
        }


        // DELETE /api/pessoas/{id}
        public async Task<bool> DeleteAsync(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null){
                return false;
            }

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();
            return true;
        }

        // GET /api/pessoas/{id} -> para buscar enquanto carrega a rota
        public async Task<Pessoa?> GetByIdAsync(int id)
        {
            return await _context.Pessoas.FirstOrDefaultAsync(p => p.Codigo == id);
        }

    }
}
