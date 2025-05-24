using AutoMapper;
using PulseManager.Application.Dto;
using PulseManager.Application.Service.Interfaces;
using PulseManager.Domain.Entities;
using PulseManager.Infraestruture.Repositories.Interface;

namespace PulseManager.Application.Service.Implementation
{
    public class CadastroService : IUsuarioService
    {
        private readonly IMethodsRepository<Usuario> _usuarioRepository;
  
        private readonly IMapper _mapper;

        public CadastroService(
            IMethodsRepository<Usuario> usuarioRepository,
            IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<UsuarioResponseDto> CriarAsync(UsuarioRequestDto usuarioRequestDto)
        {
            
            var usuario = _mapper.Map<Usuario>(usuarioRequestDto);

            usuario.ValidarMaioridade();
            
            var login = new Login(usuarioRequestDto, usuario);


            usuario.Login = login;

            
            await _usuarioRepository.AddAsync(usuario);

            
            return _mapper.Map<UsuarioResponseDto>(usuario);
        }



        public async Task<IEnumerable<UsuarioResponseDto>> ListarTodosAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UsuarioResponseDto>>(usuarios);
        }

        public async Task<UsuarioResponseDto> BuscarPorIdAsync(Guid id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException($"Cadastro com id {id} não encontrado");

            return _mapper.Map<UsuarioResponseDto>(usuario);
        }

        public async Task AtualizarAsync(Guid id, UsuarioRequestDto dto)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException($"Cadastro com id {id} não encontrado");

            _mapper.Map(dto, usuario);
            await _usuarioRepository.UpdateAsync(usuario);
        }

        public async Task DeletarAsync(Guid id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException($"Cadastro com id {id} não encontrado");

            await _usuarioRepository.RemoveAsync(usuario);
        }
    }
}
