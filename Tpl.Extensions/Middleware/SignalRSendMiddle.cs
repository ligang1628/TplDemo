using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TplDemo.Comment;
using TplDemo.Comment.Hubs;
using TplDemo.Comment.LogHelper;

namespace TplDemo.Extensions.Middleware
{
    public class SignalRSendMiddle
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly RequestDelegate _next;
        private readonly IHubContext<ChatHub> _hubContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="hubContext"></param>
        public SignalRSendMiddle(RequestDelegate next, IHubContext<ChatHub> hubContext)
        {
            _next = next;
            _hubContext = hubContext;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (bool.Parse(Appsettings.App("Middleware", "SignalR", "Enabled")))
            {
                await _hubContext.Clients.All.SendAsync("ReceiveUpdate", LogLock.GetLogData());
            }
            await _next(context);
        }

    }
}
