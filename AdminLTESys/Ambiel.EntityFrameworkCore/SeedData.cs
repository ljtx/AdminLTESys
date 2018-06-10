using System;
using System.Linq;
using Ambiel.Domain.Entites;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Ambiel.EntityFrameworkCore
{
    public static class SeedData
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AmbielDbContext>();
                if (context.Users.Any())
                {
                    return;   // 已经初始化过数据，直接返回
                }
                
                //增加一个部门
                context.Departments.Add(
                   new Department
                   {
                       Id = "1",
                       Name = "Fonour集团总部",
                       ParentId = string.Empty
                   }
                );
                //增加一个超级管理员用户
                context.Users.Add(
                     new User
                     {
                         UserName = "admin",
                         Password = "123456", //暂不进行加密
                         Name = "超级管理员",
                         DepartmentId = "1"
                     }
                );
                //增加四个基本功能菜单
                context.Menus.AddRange(
                   new Menu
                   {
                       Name = "组织机构管理",
                       Code = "Department",
                       SerialNumber = 0,
                       ParentId = string.Empty,
                       Icon = "fa fa-link"
                   },
                   new Menu
                   {
                       Name = "角色管理",
                       Code = "Role",
                       SerialNumber = 1,
                       ParentId =string.Empty,
                       Icon = "fa fa-link"
                   },
                   new Menu
                   {
                       Name = "用户管理",
                       Code = "User",
                       SerialNumber = 2,
                       ParentId = string.Empty,
                       Icon = "fa fa-link"
                   },
                   new Menu
                   {
                       Name = "功能管理",
                       Code = "Department",
                       SerialNumber = 3,
                       ParentId = string.Empty,
                       Icon = "fa fa-link"
                   }
                );
                context.SaveChanges();
            }
        }
    }
}