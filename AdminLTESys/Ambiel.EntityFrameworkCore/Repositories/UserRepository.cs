using System;
using System.Linq;
using Ambiel.Domain.Entites;
using Ambiel.Domain.IRepositories;

namespace Ambiel.EntityFrameworkCore.Repositories
{
    public class UserRepository:AmbielRepositoryBase<User,string>, IUserRepository
    {
        public UserRepository(AmbielDbContext dbcontext) : base(dbcontext)
        {

        }
        /// <summary>
        /// 检查用户是存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>存在返回用户实体，否则返回NULL</returns>
        public User CheckUser(string userName, string password)
        {
            return _dbContext.Set<User>().FirstOrDefault(it => it.UserName == userName && it.Password == password);
        }
        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
         public User GetWithRoles(string id)
        {
            var user = _dbContext.Set<User>().FirstOrDefault(it => it.Id == id);
            if (user != null)
            {
                user.UserRoles = _dbContext.Set<UserRole>().Where(it => it.UserId == id).ToList();
            }
            return user;
        }

        public bool IsUserNameExists(string userName)
        {
            var user = _dbContext.Set<User>().FirstOrDefault(it => it.UserName == userName);
            if (user != null)
            {
                return true;
            }
            return false;
        }
    }
}