using System.ComponentModel.DataAnnotations;

namespace IgrejaDashboard.Api.DTOs
{
    public class PessoaCreateDTO
    {

        [Required(ErrorMessage = "O campo nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;


        [Required(ErrorMessage = "O campo e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "O campo sexo é obrigatório.")]
        public string Sexo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo status é obrigatório.")]
        public string Status { get; set; } = string.Empty;
    }
}
