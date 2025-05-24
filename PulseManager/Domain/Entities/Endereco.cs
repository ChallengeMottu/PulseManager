using PulseManager.Exceptions;

namespace PulseManager.Domain.Entities
{
    public class Endereco
    {
        public string Rua { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public Endereco()
        {

        }

        public void ValidaCep(string cep)
        {
            if (string.IsNullOrEmpty(cep) || cep.Length != 8) throw new InvalidArgumentException("CEP inválido");
        }
    }
}
