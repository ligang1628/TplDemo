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
            // ����ע�� Appsettings
            services.AddSingleton(new Appsettings(Configuration));

            // Swagger
            services.AddSwaggerSetup();
            // Cors ����
            services.AddCorsSetup(CorsName);
            // AutoMapper
            services.AddAutoMapperSetup();
            #region �������ݿ�
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
        /// ��װNuGet��
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
            // ������Nginx��������������·�ʽ
            //app.UseSwaggerMiddle(() => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("TplDemo.index.html"));
            // ��IIS��������ֱ������
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                // ���ݰ汾���Ƶ��� ����չʾ
                var ApiName = Appsettings.App(new string[] { "Startup", "ApiName" });
                typeof(ApiVersion).GetEnumNames().ToList().ForEach(v =>
                {
                    c.SwaggerEndpoint($"/swagger/{v}/swagger.json", $"{ ApiName}{v}");
                });
                // ·�����ã�����Ϊ�գ���ʾֱ���ڸ�������localhost:7000�����ʸ��ļ�,ע��localhost:7000/swagger�Ƿ��ʲ����ģ�ȥlaunchSettings.json��launchUrlȥ����������뻻һ��·����ֱ��д���ּ��ɣ�����ֱ��дc.RoutePrefix = "doc";
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
