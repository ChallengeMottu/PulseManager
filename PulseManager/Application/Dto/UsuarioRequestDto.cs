using System.ComponentModel.DataAnnotations;
using PulseManager.Domain.Entities;
using PulseManager.Domain.Enum;

namespace PulseManager.Application.Dto
{
    public class UsuarioRequestDto
    {
        [Required(ErrorMessage = "O Nome é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage ="O CPF é obrigatório")]
        [StringLength(11, ErrorMessage = "CPF deve ter 11 dígitos.")]
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "Informar a Filial da Mottu onde opera é obrigatório")]
        public Endereco FilialMottu { get; set; }

        [Required(ErrorMessage = "O Email é obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informar sua Função é obrigatório")]
        public FuncaoEnum Funcao { get; set; }

        [Required(ErrorMessage = "Criar uma senha é obrigatório")]
        [StringLength(10, ErrorMessage = "Sua senha deve ter 10 dígitos")]
        public string Senha { get; set; }
    }
}
