using Ambiel.AppService.MenuApp.Dtos;

namespace Ambiel.AppService.RoleApp.Dtos
{
    public class RoleMenuDto
    {
        public string RoleId { get; set; }
        public RoleDto Role { get; set; }

        public string MenuId { get; set; }
        public MenuDto Menu { get; set; }
    }
}