using Ambiel.AppService.MenuApp.Dtos;
using Ambiel.AppService.UserApp.Dtos;
using Ambiel.Domain.Entites;
using AutoMapper;

namespace Ambiel.AppService
{
    public class AmbielMapper
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UserDto, User>();
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<UserRoleDto, UserRole>();
                cfg.CreateMap<UserRole, UserRoleDto>();
                cfg.CreateMap<Menu, MenuDto>();
                cfg.CreateMap<MenuDto, Menu>();
            });
        }
    }
}