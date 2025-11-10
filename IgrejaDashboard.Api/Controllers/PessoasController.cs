using Microsoft.AspNetCore.Mvc;
using IgrejaDashboard.Api.Data;
using IgrejaDashboard.Api.Models;

namespace IgrejaDashboard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasController : ControllerBase
    {

        // GET /api/pessoas
        [HttpGet]
        public ActionResult<IEnumerable<Pessoa>> GetPessoas([FromQuery] string? search)
        {
            var pessoas = FakeDatabase.Pessoas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                pessoas = pessoas.Where(p =>
                    p.Nome.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Email.Contains(search, StringComparison.OrdinalIgnoreCase)
                );
            }

            return Ok(pessoas);
        }


        // GET /api/pessoas/dashboard
        [HttpGet("dashboard")]
        public ActionResult<object> GetDashboard()
        {
            var total = FakeDatabase.Pessoas.Count;
            var masculinos = FakeDatabase.Pessoas.Count(p => p.Sexo == "Masculino");
            var femininos = FakeDatabase.Pessoas.Count(p => p.Sexo == "Feminino");

            return Ok(new { total, masculinos, femininos });
        }


        // POST /api/pessoas
        [HttpPost]
        public ActionResult<Pessoa> AddPessoa(Pessoa pessoa)
        {
            var nextId = FakeDatabase.Pessoas.Max(p => p.Codigo) + 1;
            pessoa.Codigo = nextId;
            FakeDatabase.Pessoas.Add(pessoa);

            return CreatedAtAction(nameof(GetPessoas), new { id = pessoa.Codigo }, pessoa);
        }


        // PUT /api/pessoas/{id}
        [HttpPut("{id}")]
        public IActionResult UpdatePessoa(int id, Pessoa pessoa)
        {
            var existing = FakeDatabase.Pessoas.FirstOrDefault(p => p.Codigo == id);
            if (existing == null)
                return NotFound();

            existing.Nome = pessoa.Nome;
            existing.Email = pessoa.Email;
            existing.Sexo = pessoa.Sexo;
            existing.Status = pessoa.Status;

            return NoContent();
        }


        // DELETE /api/pessoas/{id}
        [HttpDelete("{id}")]
        public IActionResult DeletePessoa(int id)
        {
            var pessoa = FakeDatabase.Pessoas.FirstOrDefault(p => p.Codigo == id);
            if (pessoa == null)
                return NotFound();

            FakeDatabase.Pessoas.Remove(pessoa);
            return NoContent();
        }
    }
}
