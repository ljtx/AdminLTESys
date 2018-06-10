using System.Collections.Generic;
using System.Linq;
using Ambiel.AppService.MenuApp.Dtos;
using Ambiel.Domain.Entites;
using Ambiel.Domain.IRepositories;
using AutoMapper;

namespace Ambiel.AppService.MenuApp
{
    public class MenuAppService:IMenuAppService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        public MenuAppService(IMenuRepository menuRepository, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _menuRepository = menuRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        public List<MenuDto> GetAllList()
        {
            var menus = _menuRepository.GetAllList().OrderBy(it=>it.SerialNumber);
            //使用AutoMapper进行实体转换
            return Mapper.Map<List<MenuDto>>(menus);
        }
        public List<MenuDto> GetMenusByParent(string parentId, int startPage, int pageSize, out int rowCount)
        {
            var menus = _menuRepository.LoadPageList(startPage, pageSize, out rowCount, it => it.ParentId == parentId, it => it.SerialNumber);
            return Mapper.Map<List<MenuDto>>(menus);
        }
        public bool InsertOrUpdate(MenuDto dto)
        {
            var menu = _menuRepository.InsertOrUpdate(Mapper.Map<Menu>(dto));
            return menu == null ? false : true;
        }
        public void DeleteBatch(List<string> ids)
        {
            _menuRepository.Delete(it => ids.Contains(it.Id));
        }

        public void Delete(string id)
        {
            _menuRepository.Delete(id);
        }

        public MenuDto Get(string id)
        {
            return Mapper.Map<MenuDto>(_menuRepository.Get(id));
        }
        public List<MenuDto> GetMenusByUser(string userId)
        {
            List<MenuDto> result = new List<MenuDto>();
            var allMenus = _menuRepository.GetAllList(it=>it.Type == 0).OrderBy(it => it.SerialNumber);
            if (userId == string.Empty) //超级管理员
                return Mapper.Map<List<MenuDto>>(allMenus);
            var user = _userRepository.GetWithRoles(userId);
            if (user == null)
                return result;
            var userRoles = user.UserRoles;
            List<string> menuIds = new List<string>();
            foreach (var role in userRoles)
            {
                menuIds = menuIds.Union(_roleRepository.GetAllMenuListByRole(role.RoleId)).ToList();
            }
            allMenus = allMenus.Where(it => menuIds.Contains(it.Id)).OrderBy(it => it.SerialNumber);
            return Mapper.Map<List<MenuDto>>(allMenus);
        }
    }
}