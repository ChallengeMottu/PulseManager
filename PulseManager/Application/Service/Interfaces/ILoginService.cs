using System.Globalization;
using PulseManager.Application.Dto;

namespace PulseManager.Application.Service.Interfaces
{
    public interface ILoginService
    {
        Task<LoginResponseDto> AutenticarAsync(LoginRequestDto loginRequestDto);

        Task<IEnumerable<LoginResponseDto>> ListarTodosAsync();

        Task<LoginResponseDto> BuscarPorCpfAsync(string cpf);

        Task<LoginResponseDto> BuscarPorIdAsync(Guid id);

        Task AtualizarSenhaAsync(Guid id, string novaSenha);

        Task DeleteAsync(Guid id);
        Task DesbloquearUsuarioAsync(Guid id);
    }
}
