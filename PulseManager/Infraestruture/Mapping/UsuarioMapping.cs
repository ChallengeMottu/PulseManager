using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseManager.Domain.Entities;

namespace PulseManager.Infraestruture.Mapping
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        
        public void Configure(EntityTypeBuilder<Usuario> builder) {
            builder.ToTable("Usuario");
            
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .Property(x => x.Cpf)
                .HasMaxLength(11)
                .IsRequired();

            builder
                .Property(x => x.DataNascimento)
                .IsRequired();

            builder
                .Property(x => x.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(x => x.Funcao)
                .IsRequired();

            builder.OwnsOne(x => x.FilialMottu, endereco =>
            {
                endereco.Property(e => e.Rua).HasColumnName("Rua").HasMaxLength(100).IsRequired();
                endereco.Property(e => e.Complemento).HasColumnName("Complemento").HasMaxLength(50);
                endereco.Property(e => e.Bairro).HasColumnName("Bairro").HasMaxLength(100).IsRequired();
                endereco.Property(e => e.Cidade).HasColumnName("Cidade").HasMaxLength(100).IsRequired();
                endereco.Property(e => e.Estado).HasColumnName("Estado").HasMaxLength(50).IsRequired();
                endereco.Property(e => e.Cep).HasColumnName("Cep").HasMaxLength(9).IsRequired();
            });

            builder
                .Property(x => x.DataCadastro)
                .HasDefaultValueSql("SYSDATE") 
                .IsRequired();

            builder
                .HasOne(c => c.Login)
                .WithOne(l => l.Usuario)
                .HasForeignKey<Login>(l => l.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);




        }
    }
    
}
