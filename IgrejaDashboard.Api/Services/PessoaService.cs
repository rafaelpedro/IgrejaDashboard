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
                    Status = p.Status
                })
                .ToListAsync();
        }


        // GET /api/pessoas/dashboard
        public async Task<object> GetDashboardAsync()
        {
            var total = await _context.Pessoas.CountAsync();
            var masculinos = await _context.Pessoas.CountAsync(p => p.Sexo == "Masculino");
            var femininos = await _context.Pessoas.CountAsync(p => p.Sexo == "Feminino");

            return new { total, masculinos, femininos };
        }


        // POST /api/pessoas
        public async Task<PessoaDTO> AddAsync(PessoaCreateDTO dto)
        {
            var pessoa = new Pessoa
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Sexo = dto.Sexo,
                Status = dto.Status
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            return new PessoaDTO
            {
                Codigo = pessoa.Codigo,
                Nome = pessoa.Nome,
                Email = pessoa.Email,
                Sexo = pessoa.Sexo,
                Status = pessoa.Status
            };
        }


        // PUT /api/pessoas/{id}
        public async Task<bool> UpdatePartialAsync(int id, Dictionary<string, string> updates)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if(pessoa == null){
                return false;
            }

            foreach ( var (campo, valor) in updates)
            {
                var lowerCampo = campo.ToLower();

                if(lowerCampo == "nome")
                    pessoa.Nome = valor;
                else if (lowerCampo == "email")
                    pessoa.Email = valor;
                else if (lowerCampo == "sexo")
                    pessoa.Sexo = valor;
                else if (lowerCampo == "status")
                    pessoa.Status = valor;
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
    }
}
