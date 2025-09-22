namespace PulseManager.Domain.DTOs
{
    public class ZonaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int PatioId { get; set; }
        public PatioDTO Patio { get; set; }
        public List<GatewayDTO> Gateways { get; set; } = new List<GatewayDTO>();
    }

    public class CreateZonaDTO
    {
        public string Nome { get; set; }
        public int PatioId { get; set; }
    }
}