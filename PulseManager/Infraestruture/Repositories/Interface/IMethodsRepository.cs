using PulseManager.Domain.Entities;

namespace PulseManager.Infraestruture.Repositories.Interface
{
    public interface IMethodsRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
    }
}
