using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using IgrejaDashboard.Api.Data;
using IgrejaDashboard.Api.Models;
using IgrejaDashboard.Api.Services;
using IgrejaDashboard.Api.DTOs;
using System.Threading.Tasks;

namespace IgrejaDashboard.Tests
{
    public class PessoaServiceTests
    {
        private readonly AppDbContext _context;
        private readonly PessoaService _service;

        public PessoaServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new AppDbContext(options);
            _service = new PessoaService(_context);
        }

        private void LimparBanco()
        {
            _context.Pessoas.RemoveRange(_context.Pessoas);
            _context.SaveChanges();
        }

        [Fact]
        public async Task AddAsync_DeveAdicionarPessoaCorretamente()
        {
            var dto = new PessoaCreateDTO
            {
                Nome = "Rafael Alencar Pedro",
                Email = "rafael@teste.com",
                Sexo = "Masculino",
                Status = "Membro"
            };

            var pessoaCriada = await _service.AddAsync(dto);

            pessoaCriada.Should().NotBeNull();
            pessoaCriada.Nome.Should().Be("Rafael Alencar Pedro");
            pessoaCriada.Email.Should().Be("rafael@teste.com");

            var pessoaNoBanco = await _context.Pessoas.FirstOrDefaultAsync(p => p.Email == "rafael@teste.com");
            pessoaNoBanco.Should().NotBeNull();
            pessoaNoBanco!.Sexo.Should().Be("Masculino");
            pessoaNoBanco.Status.Should().Be(SituacaoPessoa.Membro);

            LimparBanco();
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarListaDePessoas()
        {
            _context.Pessoas.AddRange(
                new Pessoa { Nome = "Mayza", Email = "mayza@teste.com", Sexo = "Feminino", Status = SituacaoPessoa.Membro },
                new Pessoa { Nome = "Pedro", Email = "pedro@teste.com", Sexo = "Masculino", Status = SituacaoPessoa.Visitante }
            );
            await _context.SaveChangesAsync();

            var pessoas = await _service.GetAllAsync(null);
            pessoas.Should().HaveCount(2);

            LimparBanco();
        }

        [Fact]
        public async Task UpdateAsync_DeveAtualizarDadosDaPessoaCorretamente()
        {
            var pessoa = new Pessoa
            {
                Nome = "Mayza Hirose",
                Email = "mayza@teste.com",
                Sexo = "Feminino",
                Status = SituacaoPessoa.Membro
            };
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var dto = new PessoaUpdateDTO
            {
                Nome = "Rosilene Alencar Pedro",
                Email = "rosi@teste.com",
                Sexo = "Feminino",
                Status = "Visitante"
            };

            var resultado = await _service.UpdateAsync(pessoa.Codigo, dto);

            resultado.Should().BeTrue("a atualização deve retornar verdadeiro");

            var pessoaAtualizada = await _context.Pessoas.FindAsync(pessoa.Codigo);
            pessoaAtualizada.Should().NotBeNull();
            pessoaAtualizada!.Nome.Should().Be("Rosilene Alencar Pedro");
            pessoaAtualizada.Email.Should().Be("rosi@teste.com");
            pessoaAtualizada.Status.Should().Be(SituacaoPessoa.Visitante);

            LimparBanco();
        }
    }
}
