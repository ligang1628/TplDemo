using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Extensions.Middleware
{
    public static class MiddlewareHelper
    {
        /// <summary>
        /// 请求响应中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseReuestResponseLog(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequRespLogMildd>();
        }

        /// <summary>
        /// SignalR中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSignalRSendMiddle(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SignalRSendMiddle>();
        }

        /// <summary>
        /// 异常处理中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionHandlerMidd(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlerMidd>();
        }

        /// <summary>
        /// IP请求中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseIPLogMildd(this IApplicationBuilder app)
        {
            return app.UseMiddleware<IPLogMildd>();
        }
    }
}
