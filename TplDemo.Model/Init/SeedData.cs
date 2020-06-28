using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.Comment;
using TplDemo.Model.Context;
using TplDemo.Model.Models;

namespace TplDemo.Model.Init
{
    public static class SeedData
    {
        public static async void Initialize(IServiceProvider provider)
        {
            if (bool.Parse(Appsettings.App(new string[] { "Database", "MSSQL", "Enable" })))
            {
                using var context = new MSSQLContext(provider.GetRequiredService<DbContextOptions<MSSQLContext>>());
                if (await context.UserRole.AnyAsync())
                {
                    // 已存在数据
                    return;
                }
                // 初始化操作
                await context.UserRole.AddAsync(new UserRole { Name = "admin", Enabled = true, Description = "管理员" });

                await context.UserInfo.AddAsync(new UserInfo { Name = "猫", Password = "123456", Email = "123@qq.com", Address = "", Age = 23, Birth = DateTime.Now, CreateTime = DateTime.Now, IP = "", Sex = true });
                await context.SaveChangesAsync();

                await context.UserRoleRel.AddAsync(new UserRoleRel { RoleId = 1, UserId = 1 });

                await context.UserModule.AddAsync(new UserModule { MId = "0001", Name = "测试", Desc = "", Url = "", ParentId = "", Level = "1", Icon = "", Sequence = "1" });
                await context.SaveChangesAsync();
                await context.UserModuleRel.AddAsync(new UserModuleRel { MId = 1, RId = 1, Status = true });
                await context.SaveChangesAsync();
                Console.WriteLine("MSSQL初始化完成");
            }
            else if (bool.Parse(Appsettings.App(new string[] { "Database", "MYSQL", "Enable" })))
            {
                using var context = new MYSQLContext(provider.GetRequiredService<DbContextOptions<MYSQLContext>>());
                if (await context.UserRole.AnyAsync())
                {
                    // 已存在数据
                    return;
                }
                // 初始化操作
                // 初始化操作
                await context.UserRole.AddAsync(new UserRole { Name = "admin", Enabled = true, Description = "管理员" });

                await context.UserInfo.AddAsync(new UserInfo { Name = "猫", Password = "123456", Email = "123@qq.com", Address = "", Age = 23, Birth = DateTime.Now, CreateTime = DateTime.Now, IP = "", Sex = true });
                await context.SaveChangesAsync();

                await context.UserRoleRel.AddAsync(new UserRoleRel { RoleId = 1, UserId = 1 });

                await context.UserModule.AddAsync(new UserModule { MId = "0001", Name = "测试", Desc = "", Url = "", ParentId = "", Level = "1", Icon = "", Sequence = "1" });
                await context.SaveChangesAsync();
                await context.UserModuleRel.AddAsync(new UserModuleRel { MId = 1, RId = 1, Status = true });
                await context.SaveChangesAsync();
                Console.WriteLine("MYSQL初始化完成");
            }
        }

    }
}
