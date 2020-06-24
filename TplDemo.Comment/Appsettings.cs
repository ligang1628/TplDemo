using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.IO;
using System.Linq;

namespace TplDemo.Comment
{
    /// <summary>
    /// Appsettings.json 操作类
    /// 通过IConfiguration
    /// 需安装 
    /// Microsoft.Extensions.Configuration
    /// Microsoft.Extensions.Abstract
    /// Microsoft.Extensions.Configuration.Json
    /// </summary>
    public class Appsettings
    {
        private static IConfiguration Configuration { get; set; }

        public Appsettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public Appsettings(string contentPath)
        {
            string path = "appsettings.json";

            Configuration = new ConfigurationBuilder().SetBasePath(contentPath)
                //这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
                .Add(new JsonConfigurationSource { Path = path, Optional = false, ReloadOnChange = true })
                .Build();
        }

        /// <summary>
        /// 获取appsettings.json下的值
        /// </summary>
        /// <param name="sections">JSON 节点</param>
        /// <returns></returns>
        public static string App(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception)
            {
            }
            return "";
        }

    }
}
