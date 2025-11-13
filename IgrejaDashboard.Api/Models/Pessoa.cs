using System.ComponentModel.DataAnnotations;

namespace IgrejaDashboard.Api.Models
{
    public class Pessoa
    {
        [Key]
        public int Codigo { get; set; }
        [Required]
        public string Nome { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Sexo { get; set; }=string.Empty;
        [Required]
        public SituacaoPessoa Status { get; set; } = SituacaoPessoa.Visitante;
    }
}
