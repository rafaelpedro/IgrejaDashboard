using Microsoft.AspNetCore.Mvc;
using IgrejaDashboard.Api.Models;
using IgrejaDashboard.Api.Services;
using IgrejaDashboard.Api.DTOs;

namespace IgrejaDashboard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasController : ControllerBase
    {
        private readonly PessoaService _service;

        public PessoasController(PessoaService service)
        {
            _service = service;
        }

        // GET /api/pessoas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas([FromQuery] string? search)
        {
            var pessoas = await _service.GetAllAsync(search);
            return Ok(pessoas);
        }

        // GET /api/pessoas/dashboard
        [HttpGet("dashboard")]
        public async Task<ActionResult<object>> GetDashboard()
        {
            var data = await _service.GetDashboardAsync();
            return Ok(data);
        }

        // POST /api/pessoas
        [HttpPost]
        public async Task<ActionResult<Pessoa>> AddPessoa(PessoaCreateDTO dto)
        {
            var pessoa = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetPessoas), new { id = pessoa.Codigo }, pessoa);
        }

        // PUT /api/pessoas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePessoa(int id, [FromBody] Dictionary<string, string> updates)
        {
            var updated = await _service.UpdatePartialAsync(id, updates);
            if (!updated) { 
                return NotFound();
            }
            return NoContent();
        }

        // DELETE /api/pessoas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var delete = await _service.DeleteAsync(id);
            if (!delete)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
