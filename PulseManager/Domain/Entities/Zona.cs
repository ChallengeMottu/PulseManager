namespace PulseManager.Models
{
    public class Zona
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int PatioId { get; set; }
        public Patio Patio { get; set; }
        public List<Gateway> Gateways { get; set; } = new List<Gateway>();
    }
}