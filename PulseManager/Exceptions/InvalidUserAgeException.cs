using System.Security.Cryptography.X509Certificates;

namespace PulseManager.Exceptions
{
    public class InvalidUserAgeException : Exception
    {
        public InvalidUserAgeException() : base("O colaborador deve ser maior de idade") { }
            public InvalidUserAgeException(string message) : base(message){}
        
    }
}
