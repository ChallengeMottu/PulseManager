using Microsoft.EntityFrameworkCore;
using PulseManager.Domain.Entities;
using PulseManager.Infraestruture.Mapping;

namespace PulseManager.Infraestruture.Context
{
    public class ManagerDbContext(DbContextOptions<ManagerDbContext> options): DbContext(options)

    {

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Login> Logins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMapping());
            modelBuilder.ApplyConfiguration(new LoginMapping());
        }
    }
}
