using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseManager.Domain.Entities;

namespace PulseManager.Infraestruture.Mapping
{
    public class ZonaMapping : IEntityTypeConfiguration<Zona>
    {
        public void Configure(EntityTypeBuilder<Zona> builder)
        {
            builder.ToTable("Zonas");
            builder.HasKey(z => z.Id);
            builder.Property(z => z.Nome).IsRequired().HasMaxLength(100);
            builder.Property(z => z.PatioId).IsRequired();

            // Relacionamento 1:N com Gateways
            builder.HasMany(z => z.Gateways)
                .WithOne(g => g.Zona)
                .HasForeignKey(g => g.ZonaId);
        }
    }
}