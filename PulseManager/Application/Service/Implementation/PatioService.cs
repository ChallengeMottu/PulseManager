using AutoMapper;
using PulseManager.Application.Service.Interfaces;
using PulseManager.Domain.DTOs;
using PulseManager.Domain.Entities;
using PulseManager.Infraestruture.Repositories.Interface;

namespace PulseManager.Application.Service.Implementation
{
    public class PatioService : IPatioService
    {
        private readonly IPatioRepository _patioRepository;
        private readonly IMapper _mapper;

        public PatioService(IPatioRepository patioRepository, IMapper mapper)
        {
            _patioRepository = patioRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PatioDTO>> GetAllPatiosAsync(int page = 1, int pageSize = 10)
        {
            var patios = await _patioRepository.GetAllAsync(page, pageSize);
            return _mapper.Map<IEnumerable<PatioDTO>>(patios);
        }

        public async Task<PatioDTO> GetPatioByIdAsync(int id)
        {
            var patio = await _patioRepository.GetByIdAsync(id);
            return _mapper.Map<PatioDTO>(patio);
        }

        public async Task<PatioDTO> CreatePatioAsync(CreatePatioDTO patioDto)
        {
            var patio = _mapper.Map<Patio>(patioDto);
            await _patioRepository.AddAsync(patio);
            return _mapper.Map<PatioDTO>(patio);
        }

        public async Task UpdatePatioAsync(int id, UpdatePatioDTO patioDto)
        {
            var patio = await _patioRepository.GetByIdAsync(id);
            _mapper.Map(patioDto, patio);
            await _patioRepository.UpdateAsync(patio);
        }

        public async Task DeletePatioAsync(int id)
        {
            await _patioRepository.DeleteAsync(id);
        }
    }
}