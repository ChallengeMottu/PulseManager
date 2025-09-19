using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseManager.Domain.Entities;

namespace PulseManager.Infraestruture.Mapping
{
    public class PatioMapping : IEntityTypeConfiguration<Patio>
    {
        public void Configure(EntityTypeBuilder<Patio> builder)
        {
            builder.ToTable("Patios");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Nome).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Localizacao).IsRequired().HasMaxLength(200);
            builder.Property(p => p.CapacidadeMaxima).IsRequired();

            // Relacionamento 1:N com Zonas
            builder.HasMany(p => p.Zonas)
                .WithOne(z => z.Patio)
                .HasForeignKey(z => z.PatioId);
        }
    }
}