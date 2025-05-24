using AutoMapper;
using PulseManager.Application.Dto;
using PulseManager.Application.Service.Interfaces;
using PulseManager.Domain.Entities;
using PulseManager.Infraestruture.Repositories.Interface;

namespace PulseManager.Application.Service.Implementation
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IMapper _mapper;

        public LoginService(ILoginRepository loginRepository, IMapper mapper)
        {
            _loginRepository = loginRepository;
            _mapper = mapper;
        }

        public async Task<LoginResponseDto> BuscarPorCpfAsync(string cpf)
        {
            var login = await _loginRepository.GetByCpfAsync(cpf);
            if (login == null)
                throw new KeyNotFoundException("Login não encontrado.");

            return _mapper.Map<LoginResponseDto>(login);
        }

        public async Task<LoginResponseDto> BuscarPorIdAsync(Guid id)
        {
            var login = await _loginRepository.GetByIdAsync(id);
            if (login == null)
                throw new KeyNotFoundException("Login não encontrado.");

            return _mapper.Map<LoginResponseDto>(login);
        }

        public async Task AtualizarSenhaAsync(Guid id, string novaSenha)
        {
            var login = await _loginRepository.GetByIdAsync(id);
            if (login == null)
                throw new KeyNotFoundException("Login não encontrado.");

            login.DefinirSenha(novaSenha);
            await _loginRepository.UpdateAsync(login);
        }

        public async Task DeleteAsync(Guid id)
        {
            var login = await _loginRepository.GetByIdAsync(id);
            if (login == null)
                throw new KeyNotFoundException("Login não encontrado.");

            await _loginRepository.RemoveAsync(login);
        }

        public async Task<LoginResponseDto> AutenticarAsync(LoginRequestDto loginRequestDto)
        {
            var login = await _loginRepository.GetByCpfAsync(loginRequestDto.NumeroCpf);

            if (login == null)
                throw new UnauthorizedAccessException("CPF ou senha inválidos.");

            if (login.EstaBloqueado())
                throw new InvalidOperationException("Usuário bloqueado por múltiplas tentativas de login falhas.");

            bool autenticado = login.VerificarSenha(loginRequestDto.Senha);

            await _loginRepository.UpdateAsync(login);

            if (!autenticado)
                throw new UnauthorizedAccessException("CPF ou senha inválidos.");

            return _mapper.Map<LoginResponseDto>(login);
        }


        public async Task<IEnumerable<LoginResponseDto>> ListarTodosAsync()
        {
            var logins = await _loginRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<LoginResponseDto>>(logins);
        }


        public async Task DesbloquearUsuarioAsync(Guid id)
        {
            var login = await _loginRepository.GetByIdAsync(id);
            if (login == null)
                throw new KeyNotFoundException("Login não encontrado.");

            login.Desbloquear();
            await _loginRepository.UpdateAsync(login);
        }

    }
}
