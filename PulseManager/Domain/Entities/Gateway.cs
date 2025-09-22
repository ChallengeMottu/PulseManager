namespace PulseManager.Domain.Entities
{
    public class Gateway
    {
        public int Id { get; set; }
        public string Identificador { get; set; }
        public string Tipo { get; set; } // Ex: "Entrada", "Saída", "Monitoramento"
        public bool Ativo { get; set; }
        public int ZonaId { get; set; }
        public Zona Zona { get; set; }
    }
}