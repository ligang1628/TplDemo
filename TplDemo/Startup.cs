using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using TplDemo.Comment;
using TplDemo.Extensions.ServiceExtensions;
using TplDemo.Extensions.ServiceExtensions.AutofacModule;
using TplDemo.Extensions.ServiceExtensions.AutoMap;
using TplDemo.Extensions.ServiceExtensions.Database;
using TplDemo.Extensions.ServiceExtensions.Log;
using TplDemo.Helper.Swagger;

namespace TplDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public string CorsName = "Tpl";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 单例注入 Appsettings
            services.AddSingleton(new Appsettings(Configuration));

            // Swagger
            services.AddSwaggerSetup();
            // Cors 跨域
            services.AddCorsSetup(CorsName);
            // AutoMapper
            services.AddAutoMapperSetup();
            #region 连接数据库
            if (bool.Parse(Appsettings.App(new string[] { "Database", "MSSQL", "Enable" })))
            {
                services.AddMSSQLSetup();
            }
            else
            {
                services.AddMYSQLSetup();
            }
            #endregion
            services.AddControllers();
        }

        /// <summary>
        /// 安装NuGet包
        /// Autofac.Extensions.DependencyInjection
        /// Autofac.Extras.DynamicProxy
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region Swagger
            // 若采用Nginx发布，则采用以下方式
            //app.UseSwaggerMiddle(() => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("TplDemo.index.html"));
            // 若IIS发布，或直接运行
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                // 根据版本名称倒序 遍历展示
                var ApiName = Appsettings.App(new string[] { "Startup", "ApiName" });
                typeof(ApiVersion).GetEnumNames().ToList().ForEach(v =>
                {
                    c.SwaggerEndpoint($"/swagger/{v}/swagger.json", $"{ ApiName}{v}");
                });
                // 路径配置，设置为空，表示直接在根域名（localhost:7000）访问该文件,注意localhost:7000/swagger是访问不到的，去launchSettings.json把launchUrl去掉，如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "doc";
                c.RoutePrefix = "";
            });
            #endregion
            //Cors
            app.UseCors(CorsName);

            logger.AddLog4Net();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
