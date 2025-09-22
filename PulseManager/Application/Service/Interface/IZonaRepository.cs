using PulseManager.Domain.Entities;

namespace PulseManager.Infraestruture.Repositories.Interface
{
    public interface IZonaRepository : IGenericRepository<Zona>
    {
        Task<IEnumerable<Zona>> GetAllWithPatioAndGatewaysAsync(int page = 1, int pageSize = 10);
        Task<Zona> GetByIdWithPatioAndGatewaysAsync(int id);
        Task<IEnumerable<Zona>> GetByPatioIdAsync(int patioId);
        Task<int> GetTotalCountAsync();
    }
}