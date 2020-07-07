using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TplDemo.Comment;

namespace TplDemo.Helper.Swagger
{
    public static class SwaggerSetup
    {
        /// <summary>
        /// 扩展 Swagger 启动服务
        /// 依赖 Microsoft.Extensions.DependencyInjection.Abstractions(V:3.1.5)
        /// Swashbuckle.AspNetCore.Swagger
        /// Swashbuckle.AspNetCore.SwaggerGen
        /// Swashbuckle.AspNetCore.SwaggerUI
        /// </summary>
        /// <param name="service"></param>
        public static void AddSwaggerSetup(this IServiceCollection service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            string basePath = AppContext.BaseDirectory;
            var apiName = Appsettings.App(new string[] { "Startup", "ApiName" });

            service.AddSwaggerGen(c =>
            {
                typeof(ApiVersion).GetEnumNames().ToList().ForEach(v =>
                {
                    c.SwaggerDoc(v, new OpenApiInfo
                    {
                        Version = v,
                        Title = $"{apiName} 接口文档",
                        Description = $"{apiName} HTTP API {v}",
                        Contact = new OpenApiContact { Name = apiName, Email = "1628300708@qq.com", Url = new Uri("https://www.ligang.info") },
                        License = new OpenApiLicense
                        {
                            Name = $"{apiName} 官方文档"
                        }
                    });
                    c.OrderActionsBy(d => d.RelativePath);
                    
                });

                try
                {
                    var xml = Path.Combine(basePath, "TplDemo.xml");
                    // 这个是Controller的注释,第二个参数默认为false,需要修改为true
                    c.IncludeXmlComments(xml, true);
                }
                catch (Exception)
                {
                    // 错误日志
                }
                
                // JWT Bearer认证

            });
        }
    }
}
