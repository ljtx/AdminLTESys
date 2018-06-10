using System;
using Ambiel.Domain.Entites;
using Ambiel.Domain.IRepositories;

namespace Ambiel.EntityFrameworkCore.Repositories
{
    public class DepartmentRepository:AmbielRepositoryBase<Department,string>, IDepartmentRepository
    {
        public DepartmentRepository(AmbielDbContext dbcontext) : base(dbcontext)
        {

        }
    }
}