using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.Comment;
using TplDemo.Model.Context;

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

            }
        }
    }
}
