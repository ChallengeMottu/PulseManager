using PulseManager.Domain.Entities;

namespace PulseManager.Infraestruture.Repositories.Interface
{
    public interface IPatioRepository : IGenericRepository<Patio>
    {
        Task<IEnumerable<Patio>> GetAllWithZonasAndGatewaysAsync(int page = 1, int pageSize = 10);
        Task<Patio> GetByIdWithZonasAndGatewaysAsync(int id);
        Task<int> GetTotalCountAsync();
    }
}