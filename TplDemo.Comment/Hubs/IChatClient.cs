using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TplDemo.Comment.Hubs
{
    public interface IChatClient
    {
        /// <summary>
        /// SignalR接收消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task ReceiveMessage(object message);
        /// <summary>
        /// SignalR接收消息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task ReceiveMessage(string user, object message);
        Task ReceiveUpdate(object message);
    }
}
