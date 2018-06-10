using System.Collections.Generic;
using System.Linq;
using Ambiel.Domain.Entites;
using Ambiel.Domain.IRepositories;

namespace Ambiel.EntityFrameworkCore.Repositories
{
    public class RoleRepository:AmbielRepositoryBase<Role,string>, IRoleRepository
    {
        public RoleRepository(AmbielDbContext dbcontext) : base(dbcontext)
        {

        }
        /// <summary>
        /// 根据角色获取权限
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllMenuListByRole(string roleId)
        {
            var roleMenus = _dbContext.Set<RoleMenu>().Where(it => it.RoleId == roleId);
            var menuIds = from t in roleMenus select t.MenuId;
            return menuIds.ToList();
        }

        /// <summary>
        /// 更新角色权限关联关系
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="roleMenus">角色权限集合</param>
        /// <returns></returns>
        public bool UpdateRoleMenu(string roleId, List<RoleMenu> roleMenus)
        {
            var oldDatas = _dbContext.Set<RoleMenu>().Where(it => it.RoleId == roleId).ToList();
            oldDatas.ForEach(it => _dbContext.Set<RoleMenu>().Remove(it));
            _dbContext.SaveChanges();
            _dbContext.Set<RoleMenu>().AddRange(roleMenus);
            _dbContext.SaveChanges();
            return true;
        }
    }
}