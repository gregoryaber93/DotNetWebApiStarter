using Application.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterUserDto, User>()
                .ForMember(u => u.Email, c => c.MapFrom(s => s.Email))
                .ForMember(u => u.Name, c => c.MapFrom(s => s.Email))
                .ForMember(u => u.RoleId, c => c.MapFrom(s => 2))
                .ForMember(u => u.RegisterCode, c => c.MapFrom(s => Guid.NewGuid().ToString()))
                .ForMember(u => u.IsActive, c => c.MapFrom(s => false));
        }
    }
}