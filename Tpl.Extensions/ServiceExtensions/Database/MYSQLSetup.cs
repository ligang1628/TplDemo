using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.Comment;
using TplDemo.Model.Context;

namespace TplDemo.Extensions.ServiceExtensions.Database
{
    public static class MYSQLSetup
    {
        public static void AddMYSQLSetup(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            string conn = Appsettings.App(new string[] { "Database", "MYSQL", "Connection" });
            if (string.IsNullOrWhiteSpace(conn))
            {
                string msg = "请检查连接字符串";
                Console.WriteLine(msg);
                throw new ArgumentNullException(msg);
            }
            services.AddDbContext<MYSQLContext>(options =>
            {
                options.UseMySql(conn);
            });
        }
    }
}
