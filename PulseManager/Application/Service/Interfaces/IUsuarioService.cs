using PulseManager.Application.Dto;
using PulseManager.Domain.Entities;

namespace PulseManager.Application.Service.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioResponseDto> CriarAsync(UsuarioRequestDto usuarioRequestDto);
        Task<IEnumerable<UsuarioResponseDto>> ListarTodosAsync();
        Task<UsuarioResponseDto> BuscarPorIdAsync(Guid id);

        Task AtualizarAsync(Guid id, UsuarioRequestDto dto);
        Task DeletarAsync(Guid id);

    }
}
