using System.Net;
using System.Net.Http.Json;
using IgrejaDashboard.Api;
using IgrejaDashboard.Api.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;

namespace IgrejaDashboard.Tests
{
    public class PessoaControllerTests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PessoaControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetPessoas_DeveRetornarStatus200()
        {

            var response = await _client.GetAsync("/api/pessoas");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PostPessoa_DeveCriarNovaPessoa()
        {

            var novaPessoa = new PessoaCreateDTO
            {
                Nome = "João Teste",
                Email = "joao@teste.com",
                Sexo = "Masculino",
                Status = "Membro"
            };


            var response = await _client.PostAsJsonAsync("/api/pessoas", novaPessoa);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task PostPessoa_ComDadosInvalidos_DeveRetornar400()
        {
            var dtoInvalido = new PessoaCreateDTO
            {
                Nome = "",
                Email = "invalido",
                Sexo = "",
                Status = ""
            };

            var response = await _client.PostAsJsonAsync("/api/pessoas", dtoInvalido);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeletePessoa_Inexistente_DeveRetornar404()
        {
            var response = await _client.DeleteAsync("/api/pessoas/9999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
