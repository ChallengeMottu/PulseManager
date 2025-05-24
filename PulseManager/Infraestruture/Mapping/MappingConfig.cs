using AutoMapper;
using PulseManager.Application.Dto;
using PulseManager.Domain.Entities;


namespace PulseManager.Infraestruture.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig() {
            CreateMap<UsuarioRequestDto, Usuario>();
            CreateMap<Usuario, UsuarioResponseDto>();
            CreateMap<Login, LoginResponseDto>()
                .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.Usuario.Name))
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.Usuario.Id))
                .ForMember(dest => dest.TentativasLogin, opt => opt.MapFrom(src => src.TentativasLogin));


            CreateMap<LoginRequestDto, Login>();

        }
    }
}
