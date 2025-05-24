namespace PulseManager.Application.Dto
{
    public class LoginResponseDto
    {
        public Guid Id { get; set; }
        public string NumeroCpf { get; set; }
        public int TentativasLogin { get; set; }
        public string NomeUsuario { get; set; }

        public Guid UsuarioId { get; set; }
    }
}
