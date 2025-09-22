using PulseManager.Domain.Entities;

namespace PulseManager.Infraestruture.Repositories.Interface
{
    public interface IGatewayRepository : IGenericRepository<Gateway>
    {
        Task<IEnumerable<Gateway>> GetAllWithZonaAndPatioAsync(int page = 1, int pageSize = 10);
        Task<Gateway> GetByIdWithZonaAndPatioAsync(int id);
        Task<IEnumerable<Gateway>> GetByZonaIdAsync(int zonaId);
        Task<int> GetTotalCountAsync();
    }
}