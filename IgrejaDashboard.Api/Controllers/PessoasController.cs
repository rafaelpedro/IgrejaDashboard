using Microsoft.AspNetCore.Mvc;
using IgrejaDashboard.Api.Data;
using IgrejaDashboard.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace IgrejaDashboard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PessoasController(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/pessoas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas([FromQuery] string? search)
        {
            var query = _context.Pessoas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Nome.Contains(search) ||
                    p.Email.Contains(search));
            }

            return await query.ToListAsync();
        }

        // GET /api/pessoas/dashboard
        [HttpGet("dashboard")]
        public async Task<ActionResult<object>> GetDashboard()
        {
            var total = await _context.Pessoas.CountAsync();
            var masculinos = await _context.Pessoas.CountAsync(p => p.Sexo == "Masculino");
            var femininos = await _context.Pessoas.CountAsync(p => p.Sexo == "Feminino");

            return Ok(new { total, masculinos, femininos });
        }

        // POST /api/pessoas
        [HttpPost]
        public async Task<ActionResult<Pessoa>> AddPessoa(Pessoa pessoa)
        {
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPessoas), new { id = pessoa.Codigo }, pessoa);
        }

        // PUT /api/pessoas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePessoa(int id, [FromBody] JsonElement updates)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null)
                return NotFound();

            foreach (var prop in updates.EnumerateObject())
            {
                var campo = prop.Name.ToLower();

                if (campo == "nome")
                    pessoa.Nome = prop.Value.GetString() ?? pessoa.Nome;
                else if (campo == "email")
                    pessoa.Email = prop.Value.GetString() ?? pessoa.Email;
                else if (campo == "sexo")
                    pessoa.Sexo = prop.Value.GetString() ?? pessoa.Sexo;
                else if (campo == "status")
                    pessoa.Status = prop.Value.GetString() ?? pessoa.Status;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE /api/pessoas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null)
                return NotFound();

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
