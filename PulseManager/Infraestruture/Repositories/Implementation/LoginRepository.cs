using Microsoft.EntityFrameworkCore;
using PulseManager.Infraestruture.Context;
using PulseManager.Infraestruture.Repositories.Interface;

namespace PulseManager.Infraestruture.Repositories.Implementation
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ManagerDbContext _context;
        private readonly DbSet<Login> _dbSet;

        public LoginRepository(ManagerDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Login>();
        }

        public async Task<Login?> GetByCpfAsync(string cpf)
        {
            return await _dbSet
                .Include(l => l.Usuario)
                .FirstOrDefaultAsync(l => l.NumeroCpf == cpf);
        }

        public async Task<IEnumerable<Login>> GetAllAsync()
        {
            return await _dbSet
                .Include(l => l.Usuario)
                .ToListAsync();
        }


        public async Task<Login?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(l => l.Usuario)
                .FirstOrDefaultAsync(l => l.Id == id);
        }


        public async Task UpdateAsync(Login login)
        {
            _dbSet.Update(login);
            await _context.SaveChangesAsync();
            _context.Entry(login).Reload();
        }

        public async Task RemoveAsync(Login login)
        {
            _dbSet.Remove(login);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(Login login)
        {
            await _dbSet.AddAsync(login);
            await _context.SaveChangesAsync();
        }
    }

}
