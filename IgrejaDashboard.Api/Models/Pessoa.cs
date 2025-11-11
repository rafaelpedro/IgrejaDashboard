using System.ComponentModel.DataAnnotations;

namespace IgrejaDashboard.Api.Models
{
    public class Pessoa
    {
        [Key]
        public int Codigo { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Sexo { get; set; }=string.Empty;
        public SituacaoPessoa Status { get; set; } = SituacaoPessoa.Visitante;
    }
}
