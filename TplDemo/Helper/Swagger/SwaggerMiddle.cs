using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TplDemo.Comment;

namespace TplDemo.Helper.Swagger
{
    public static class SwaggerMiddle
    {
        /// <summary>
        /// Swagger中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="streamHtml"></param>
        public static void UseSwaggerMiddle(this IApplicationBuilder app, Func<Stream> streamHtml)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                // 根据版本名称倒序 遍历展示
                var ApiName = Appsettings.App(new string[] { "Startup", "ApiName" });
                typeof(ApiVersion).GetEnumNames().ToList().ForEach(v =>
                {
                    c.SwaggerEndpoint($"/swagger/{v}/swagger.json", $"{ ApiName}{v}");
                });


                //将swagger首页，设置成自定义页面，记得这个字符串的写法：{项目名.index.html}
                if (streamHtml.Invoke() == null)
                {
                    var msg = "index.html的属性，必须设置成嵌入的资源";
                    throw new Exception(msg);
                }
                c.IndexStream = streamHtml;


                // 路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，去launchSettings.json把launchUrl去掉，如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "doc";
                c.RoutePrefix = "";

                c.HeadContent = @"<script async='async' id='mini-profiler' src='/profiler/includes.min.js?v=4.1.0+c940f0f28d1' data-version='4.1.0+c940f0f28d' data-path='/profiler/' data-current-id='4ec7c742-49d4-4eaf-8281-3c1e0efa8888' data-ids='4ec7c742-49d4-4eaf-8281-3c1e0efa8888' data-position='Left' data-authorized='true' data-max-traces='5' data-toggle-shortcut='Alt+P' data-trivial-milliseconds='2.0' data-ignored-duplicate-execute-types='Open,OpenAsync,Close,CloseAsync'></script>";
            });

        }
    }
}
