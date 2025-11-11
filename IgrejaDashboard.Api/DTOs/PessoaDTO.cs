namespace IgrejaDashboard.Api.DTOs
{
    public class PessoaDTO
    {
        public int Codigo { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
