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

        private void LimparBancoAsync()
        {
            _context.Pessoas.RemoveRange(_context.Pessoas);
            _context.SaveChanges();
        }


        // TESTES PARA METODO ADDASYNC
        [Fact]
        public async Task AddAsync_DeveAdicionarPessoaCorretamente()
        {
            var dto = new PessoaCreateDTO
            {
                Nome = "João Silva",
                Email = "joao@teste.com",
                Sexo = "Masculino",
                Status = "Membro"
            };

            var pessoaCriada = await _service.AddAsync(dto);

            pessoaCriada.Should().NotBeNull();
            pessoaCriada.Email.Should().Be("joao@teste.com");

            LimparBancoAsync();
        }

        [Fact]
        public async Task AddAsync_DeveConverterStatusMesmoComMinusculas()
        {
            var dto = new PessoaCreateDTO
            {
                Nome = "Maria",
                Email = "maria@teste.com",
                Sexo = "Feminino",
                Status = "visitante"
            };

            var pessoaCriada = await _service.AddAsync(dto);

            pessoaCriada.Status.Should().Be("Visitante");
            LimparBancoAsync();
        }


        [Fact]
        public async Task GetAllAsync_DeveRetornarTodasAsPessoas()
        {
            _context.Pessoas.AddRange(
                new Pessoa { Nome = "A", Email = "a@x.com", Sexo = "M", Status = SituacaoPessoa.Membro },
                new Pessoa { Nome = "B", Email = "b@x.com", Sexo = "F", Status = SituacaoPessoa.Visitante }
            );
            await _context.SaveChangesAsync();

            var pessoas = await _service.GetAllAsync(null);
            pessoas.Should().HaveCount(2);

            LimparBancoAsync();
        }

        [Fact]
        public async Task GetAllAsync_DeveFiltrarPorEmailOuNome()
        {
            _context.Pessoas.AddRange(
                new Pessoa { Nome = "Carlos", Email = "carlos@x.com", Sexo = "M", Status = SituacaoPessoa.Membro },
                new Pessoa { Nome = "Ana", Email = "ana@x.com", Sexo = "F", Status = SituacaoPessoa.Membro }
            );
            await _context.SaveChangesAsync();

            var pessoas = await _service.GetAllAsync("ana");
            pessoas.Should().ContainSingle(p => p.Nome == "Ana");

            LimparBancoAsync();
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarListaVazia_SeNaoEncontrar()
        {
            var pessoas = await _service.GetAllAsync("naoexiste");
            pessoas.Should().BeEmpty();
        }


        [Fact]
        public async Task UpdateAsync_DeveAtualizarCorretamente()
        {
            var pessoa = new Pessoa { Nome = "Lucas", Email = "lucas@x.com", Sexo = "M", Status = SituacaoPessoa.Membro };
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var dto = new PessoaUpdateDTO { Nome = "Lucas Atualizado", Status = "Visitante" };
            var resultado = await _service.UpdateAsync(pessoa.Codigo, dto);

            resultado.Should().BeTrue();

            var atualizado = await _context.Pessoas.FindAsync(pessoa.Codigo);
            atualizado!.Nome.Should().Be("Lucas Atualizado");
            atualizado.Status.Should().Be(SituacaoPessoa.Visitante);

            LimparBancoAsync();
        }

        [Fact]
        public async Task UpdateAsync_DeveRetornarFalse_SePessoaNaoExistir()
        {
            var dto = new PessoaUpdateDTO { Nome = "Fulano" };
            var resultado = await _service.UpdateAsync(999, dto);

            resultado.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_DeveIgnorarCamposNulos()
        {
            var pessoa = new Pessoa { Nome = "Antigo", Email = "antigo@x.com", Sexo = "M", Status = SituacaoPessoa.Membro };
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var dto = new PessoaUpdateDTO(); // sem campos
            var resultado = await _service.UpdateAsync(pessoa.Codigo, dto);

            resultado.Should().BeTrue();

            var atualizado = await _context.Pessoas.FindAsync(pessoa.Codigo);
            atualizado!.Nome.Should().Be("Antigo"); // Não mudou
            atualizado.Email.Should().Be("antigo@x.com");

            LimparBancoAsync();
        }


        [Fact]
        public async Task GetDashboardAsync_DeveRetornarTotaisCorretos()
        {
            LimparBancoAsync();
            _context.Pessoas.AddRange(
                new Pessoa { Nome = "M1", Email = "m1@x.com", Sexo = "Masculino", Status = SituacaoPessoa.Membro },
                new Pessoa { Nome = "F1", Email = "f1@x.com", Sexo = "Feminino", Status = SituacaoPessoa.Visitante }
            );
            await _context.SaveChangesAsync();

            var dashboard = await _service.GetDashboardAsync();

            dashboard.Should().NotBeNull();
            dashboard.Should().BeEquivalentTo(new { total = 2, masculinos = 1, femininos = 1 });

            LimparBancoAsync();
        }

    }
}
