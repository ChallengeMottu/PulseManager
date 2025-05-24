using System.ComponentModel.DataAnnotations;

namespace PulseManager.Application.Dto
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Fornecer CPF é obrigatório")]
        [StringLength(11, ErrorMessage = "CPF deve ter 11 dígitos.")]
        public string NumeroCpf { get; set; }

        [Required(ErrorMessage = "Fornecer Senha é obrigatório")]
        [StringLength(10, ErrorMessage = "Sua senha deve ter 10 dígitos")]
        public string Senha { get; set; }
    }
}
