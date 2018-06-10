using System;
using Ambiel.Domain.Entites;
using Ambiel.Domain.IRepositories;

namespace Ambiel.EntityFrameworkCore.Repositories
{
    public class MenuRepository:AmbielRepositoryBase<Menu,string>, IMenuRepository
    {
        public MenuRepository(AmbielDbContext dbcontext) : base(dbcontext)
        {

        }
    }
}