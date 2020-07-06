using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using TplDemo.Model.Init;

namespace TplDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    // 初始化种子数据
                    SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.InnerException?.ToString() ?? ex.Message);
                    throw ex;
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            // 依赖注入
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureLogging((context, logBuilder) =>
                {
                    logBuilder.AddFilter("System", LogLevel.Warning);
                    logBuilder.AddFilter("Microsoft", LogLevel.Warning);
                    logBuilder.SetMinimumLevel(LogLevel.Trace);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseNLog();
    }
}
