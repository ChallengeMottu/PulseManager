namespace PulseManager.Domain.DTOs
{
    public class GatewayDTO
    {
        public int Id { get; set; }
        public string Identificador { get; set; }
        public string Tipo { get; set; }
        public bool Ativo { get; set; }
        public int ZonaId { get; set; }
        public ZonaDTO Zona { get; set; }
    }

    public class CreateGatewayDTO
    {
        public string Identificador { get; set; }
        public string Tipo { get; set; }
        public bool Ativo { get; set; }
        public int ZonaId { get; set; }
    }
}