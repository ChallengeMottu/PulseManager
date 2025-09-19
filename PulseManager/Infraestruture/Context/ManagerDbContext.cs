using Microsoft.EntityFrameworkCore;
using PulseManager.Domain.Entities;
using PulseManager.Infraestruture.Mapping;

namespace PulseManager.Infraestruture.Context
{
    public class ManagerDbContext : DbContext
    {
        public ManagerDbContext(DbContextOptions<ManagerDbContext> options) : base(options) { }

        // Novas entidades
        public DbSet<Patio> Patios { get; set; }
        public DbSet<Zona> Zonas { get; set; }
        public DbSet<Gateway> Gateways { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplica os novos mapeamentos
            modelBuilder.ApplyConfiguration(new PatioMapping());
            modelBuilder.ApplyConfiguration(new ZonaMapping());
            modelBuilder.ApplyConfiguration(new GatewayMapping());
        }
    }
}