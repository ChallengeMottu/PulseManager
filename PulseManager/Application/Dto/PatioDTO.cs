namespace PulseManager.Domain.DTOs
{
    public class PatioDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Localizacao { get; set; }
        public int CapacidadeMaxima { get; set; }
        public List<ZonaDTO> Zonas { get; set; } = new List<ZonaDTO>();
    }

    public class CreatePatioDTO
    {
        public string Nome { get; set; }
        public string Localizacao { get; set; }
        public int CapacidadeMaxima { get; set; }
    }

    public class UpdatePatioDTO
    {
        public string Nome { get; set; }
        public string Localizacao { get; set; }
        public int CapacidadeMaxima { get; set; }
    }
}