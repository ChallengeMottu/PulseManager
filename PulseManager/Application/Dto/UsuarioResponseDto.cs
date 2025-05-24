using PulseManager.Domain.Entities;
using PulseManager.Domain.Enum;

namespace PulseManager.Application.Dto
{
    public class UsuarioResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public Endereco FilialMottu { get; set; }
        public string Email { get; set; }
        public FuncaoEnum Funcao { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    }
}
