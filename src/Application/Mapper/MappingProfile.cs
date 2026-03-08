using Application.DTO;
using AutoMapper;
using Domain.Common.Ids;
using Domain.Entities;
using Persistence.EF.DbContexts;

namespace Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterUserDto, User>()
                .ForMember(u => u.Email, c => c.MapFrom(s => s.Email))
                .ForMember(u => u.Name, c => c.MapFrom(s => s.Email))
                .ForMember(u => u.RoleId, c => c.MapFrom((s, d, _, context) => 
                {
                    var dbContext = context.Items["UserDbContext"] as UserDbContext;
                    var userRole = dbContext?.Role.FirstOrDefault(r => r.Name == "user");
                    return userRole?.Id ?? throw new Exception("User role not found");
                }))
                .ForMember(u => u.RegisterCode, c => c.MapFrom(s => Guid.NewGuid().ToString()))
                .ForMember(u => u.IsActive, c => c.MapFrom(s => false));
        }
    }
}