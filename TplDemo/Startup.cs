using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TplDemo.Comment;
using TplDemo.Extensions.ServiceExtensions;
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

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
