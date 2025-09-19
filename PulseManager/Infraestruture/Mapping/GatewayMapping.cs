using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseManager.Domain.Entities;

namespace PulseManager.Infraestruture.Mapping
{
    public class GatewayMapping : IEntityTypeConfiguration<Gateway>
    {
        public void Configure(EntityTypeBuilder<Gateway> builder)
        {
            builder.ToTable("Gateways");
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Identificador).IsRequired().HasMaxLength(50);
            builder.Property(g => g.Tipo).IsRequired().HasMaxLength(20);
            builder.Property(g => g.Ativo).IsRequired();
            builder.Property(g => g.ZonaId).IsRequired();
        }
    }
}