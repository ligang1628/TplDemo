using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TplDemo.Comment;
using TplDemo.Comment.Hubs;
using TplDemo.Comment.LogHelper;
using TplDemo.Extensions;
using TplDemo.Extensions.Middleware;
using TplDemo.Extensions.ServiceExtensions;
using TplDemo.Extensions.ServiceExtensions.AutofacModule;
using TplDemo.Extensions.ServiceExtensions.AutoMap;
using TplDemo.Extensions.ServiceExtensions.Database;
using TplDemo.Filter;
using TplDemo.Helper.Swagger;

namespace TplDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment Env)
        {
            Configuration = configuration;
            this.Env = Env;
        }

        public IConfiguration Configuration { get; }
        public string CorsName = "Tpl";
        private readonly IHostEnvironment Env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 单例注入 Appsettings
            services.AddSingleton(new Appsettings(Configuration));
            services.AddSingleton(new LogLock(Env.ContentRootPath));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            // MemoryCache
            services.AddMemoryCacheSetup();
            // 性能分析
            services.AddMiniProfilerSetup();
            // Swagger
            services.AddSwaggerSetup();
            // AutoMapper
            services.AddAutoMapperSetup();
            // Cors 跨域
            services.AddCorsSetup(CorsName);
            // 使用Signalr
            services.AddSignalR().AddNewtonsoftJsonProtocol();

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
            services.AddControllers(o =>
            {
                // 全局异常过滤
                o.Filters.Add(typeof(GlobalExceptionsFilter));
            }).AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //设置时间格式
                //options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
            });
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 请求响应日志
            app.UseReuestResponseLog();
            // SignalR
            app.UseSignalRSendMiddle();
            // 记录ip请求
            app.UseIPLogMildd();
            // 性能分析
            app.UseMiniProfiler();
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
                    c.SwaggerEndpoint($"/swagger/{v}/swagger.json", $"{ApiName}{v}");
                });
                // 路径配置，设置为空，表示直接在根域名（localhost:7000）访问该文件,注意localhost:7000/swagger是访问不到的，去launchSettings.json把launchUrl去掉，如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "doc";
                c.RoutePrefix = "";
                c.HeadContent = @"<script async='async' id='mini-profiler' src='/profiler/includes.min.js?v=4.1.0+c940f0f28d1' data-version='4.1.0+c940f0f28d' data-path='/profiler/' data-current-id='4ec7c742-49d4-4eaf-8281-3c1e0efa8888' data-ids='4ec7c742-49d4-4eaf-8281-3c1e0efa8888' data-position='Left' data-authorized='true' data-max-traces='5' data-toggle-shortcut='Alt+P' data-trivial-milliseconds='2.0' data-ignored-duplicate-execute-types='Open,OpenAsync,Close,CloseAsync'></script>";
            });
            #endregion
            // Cors 跨域
            app.UseCors(CorsName);
            // 使用静态文件
            app.UseStaticFiles();
            // 使用 cookie
            app.UseCookiePolicy();
            // 返回错误码
            app.UseStatusCodePages();
            app.UseRouting();

            // 认证
            //app.UseAuthentication();
            // 授权
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHub<ChatHub>("/api/chathub");
            });
        }
    }
}
