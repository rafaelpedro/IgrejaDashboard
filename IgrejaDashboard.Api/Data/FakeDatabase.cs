using IgrejaDashboard.Api.Models;

namespace IgrejaDashboard.Api.Data
{
    public class FakeDatabase
    {
        public static List<Pessoa> Pessoas = new()
        {
            new Pessoa { Codigo = 1, Nome = "Rafael Alencar Pedro", Email = "rafael@example.com", Sexo = "Masculino", Status = SituacaoPessoa.Membro },
            new Pessoa { Codigo = 2, Nome = "Mayza Yuri Hirose da Costa", Email = "mayza@example.com", Sexo = "Feminino", Status = SituacaoPessoa.Membro },
            new Pessoa { Codigo = 3, Nome = "Lucas Alencar Pedro", Email = "lucas@example.com", Sexo = "Masculino", Status = SituacaoPessoa.Membro },
            new Pessoa { Codigo = 1, Nome = "Gabriel Alencar Hirose", Email = "gabriel@example.com", Sexo = "Masculino", Status = SituacaoPessoa.Membro },
            new Pessoa { Codigo = 2, Nome = "Rosilene de Alencar Pedro", Email = "rosi@example.com", Sexo = "Feminino", Status = SituacaoPessoa.Membro },
            new Pessoa { Codigo = 3, Nome = "Jair Pedro", Email = "jair@example.com", Sexo = "Masculino", Status = SituacaoPessoa.Visitante }
        };
    }
}
