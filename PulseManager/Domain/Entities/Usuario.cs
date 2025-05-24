using PulseManager.Exceptions;
using PulseManager.Domain.Enum;
using PulseManager.Application.Dto;

namespace PulseManager.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Cpf { get; private set; }
        public DateTime DataNascimento { get; private set; }

        public Endereco FilialMottu { get; private set; }

        public string Email { get; private set; }

        public FuncaoEnum Funcao { get; private set; }
        public DateTime DataCadastro { get; private set; } = DateTime.UtcNow;

        public Login Login { get; set; }


        public Usuario()
        {
            
        }

        public void ValidarMaioridade()
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - DataNascimento.Year;

            if (DataNascimento > hoje.AddYears(-idade))
                idade--;

            if (idade < 18)
                throw new InvalidUserAgeException();
        }


    }


}
