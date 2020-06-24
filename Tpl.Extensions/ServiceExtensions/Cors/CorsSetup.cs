using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.Comment;

namespace TplDemo.Extensions.ServiceExtensions
{
    public static class CorsSetup
    {
        /// <summary>
        /// 跨域
        /// 依赖：Microsoft.AspNetCore.Cors
        /// 支持多个域名端口，注意端口号后不要带/斜杆：比如localhost:8000/，是错的
        /// 注意，http://127.0.0.1:1818 和 http://localhost:1818 是不一样的，尽量写两个
        /// </summary>
        /// <param name="services">服务</param>
        /// <param name="corePolicy">跨域名</param>
        public static void AddCorsSetup(this IServiceCollection services, string corePolicy)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddCors(c =>
            {
                c.AddPolicy(corePolicy, policy =>
                {
                    policy
                    .WithOrigins(Appsettings.App(new string[] { "Startup", "Cors", "IPs" }).Split(','))
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });

                // 允许任意跨域请求，也要配置中间件
                //c.AddPolicy("AllRequests",policy=> {
                //    policy.AllowAnyOrigin();
                //    policy.AllowAnyMethod();
                //    policy.AllowAnyHeader();
                //});
            });
        }
    }
}
