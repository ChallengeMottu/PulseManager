namespace PulseManager.Models
{
    public class Patio
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Localizacao { get; set; }
        public int CapacidadeMaxima { get; set; }
        public List<Zona> Zonas { get; set; } = new List<Zona>();
    }
}