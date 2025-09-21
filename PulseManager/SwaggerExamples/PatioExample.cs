using Swashbuckle.AspNetCore.Filters;

namespace PulseManager.SwaggerExamples
{
    public class PatioExample : IExamplesProvider<Patio>
    {
        public Patio GetExamples()
        {
            return new Patio
            {
                Nome = "Pátio Centro",
                Localizacao = "Centro da cidade",
                CapacidadeMaxima = 100
            };
        }
    }
}