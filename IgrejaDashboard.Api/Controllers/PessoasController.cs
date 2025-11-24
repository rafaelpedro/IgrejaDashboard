using Microsoft.AspNetCore.Mvc;
using IgrejaDashboard.Api.Models;
using IgrejaDashboard.Api.Services;
using IgrejaDashboard.Api.DTOs;
using Microsoft.EntityFrameworkCore;

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
        /// <summary>
        /// Retorna a lista de pessoas cadastradas.
        /// </summary>
        /// <param name="search">Texto para busca por nome ou e-mail.</param>
        /// <returns>Lista de pessoas filtradas.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas([FromQuery] string? search)
        {
            var pessoas = await _service.GetAllAsync(search);
            return Ok(pessoas);
        }

        // GET /api/pessoas/dashboard
        /// <summary>
        /// Retorna os valores correspondentes ao total de membros, homens e mulheres.
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Lista de valores</returns>
        [HttpGet("dashboard")]
        public async Task<ActionResult<object>> GetDashboard()
        {
            var data = await _service.GetDashboardAsync();
            return Ok(data);
        }

        // POST /api/pessoas
        /// <summary>
        /// Adiciona uma nova pessoa ao sistema.
        /// </summary>
        /// <param name="dto">Objeto contendo os dados da nova pessoa (nome, e-mail, sexo e status).</param>
        /// <returns>
        /// Retorna a pessoa criada com código 201 (Created),
        /// </returns>
        /// <response code="201">Pessoa criada com sucesso.</response>
        /// <response code="400">Dados inválidos ou mal formatados.</response>
        [HttpPost]
        public async Task<ActionResult<Pessoa>> AddPessoa(PessoaCreateDTO dto)
        {
            var pessoa = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetPessoas), new { id = pessoa.Codigo }, pessoa);
        }

        // PUT /api/pessoas/{id}
        /// <summary>
        /// Atualiza parcialmente os dados de uma pessoa existente.
        /// </summary>
        /// <param name="id">Id do membro que será atualizado.</param>
        /// <param name="dto">Objeto contendo os campos a serem atualizados (nome, e-mail, sexo ou status).</param>
        /// <returns>
        /// Retorna 204 (No Content) se a atualização for bem-sucedida,
        /// ou 404 (Not Found) se a pessoa não for encontrada.
        /// </returns>
        /// <response code="204">Atualização realizada com sucesso.</response>
        /// <response code="404">Pessoa não encontrada.</response>
        /// <response code="400">Erro na requisição (dados inválidos).</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePessoa(int id, [FromBody] PessoaUpdateDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated) { 
                return NotFound();
            }
            return NoContent();
        }

        // DELETE /api/pessoas/{id}
        /// <summary>
        /// Exclui membro pelo ID
        /// </summary>
        /// <param name="id">Id do membro para exclusão</param>
        /// <returns></returns>
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

        // GET /api/pessoas/{id}
        /// <summary>
        /// Retorna uma pessoa pelo ID.
        /// </summary>
        /// <param name="id">Id do membro buscado</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Pessoa>> GetPessoaById(int id)
        {
            var pessoa = await _service.GetByIdAsync(id);

            if (pessoa == null)
                return NotFound();

            return Ok(pessoa);
        }


    }
}
