using PulseManager.Domain.DTOs;
using PulseManager.Domain.Entities;

namespace PulseManager.Application.Service.Interfaces
{
    public interface IPatioService
    {
        Task<IEnumerable<PatioDTO>> GetAllPatiosAsync(int page = 1, int pageSize = 10);
        Task<PatioDTO> GetPatioByIdAsync(int id);
        Task<PatioDTO> CreatePatioAsync(CreatePatioDTO patioDto);
        Task UpdatePatioAsync(int id, UpdatePatioDTO patioDto);
        Task DeletePatioAsync(int id);
    }
}