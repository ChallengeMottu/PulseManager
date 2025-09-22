using Microsoft.EntityFrameworkCore;
using PulseManager.Domain.Entities;
using PulseManager.Infraestruture.Context;
using PulseManager.Infraestruture.Repositories.Interface;
namespace PulseManager.Infraestruture.Repositories.Implementation
{
    public class UsuarioRepository : IMethodsRepository<Usuario>
    {
        private readonly ManagerDbContext _context;
        private readonly DbSet<Usuario> _dbSet;

        public UsuarioRepository(ManagerDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Usuario>();
        }
        public async Task AddAsync(Usuario usuario)
        {
            await _dbSet.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Usuario?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task RemoveAsync(Usuario usuario)
        {
            _dbSet.Remove(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _dbSet.Update(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
