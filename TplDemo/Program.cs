using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TplDemo.Model.Init;

namespace TplDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
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
                    throw ex;
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            // 依赖注入
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<Startup>()
                    .ConfigureLogging((context, builder) =>
                    {
                        builder.AddFilter("System", LogLevel.Error);
                        builder.AddFilter("Microsoft", LogLevel.Error);

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "log4net.config");
                        builder.AddLog4Net(path);
                    });
                });
    }
}
