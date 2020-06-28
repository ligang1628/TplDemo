using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TplDemo.IRepository;
using TplDemo.IServices;
using TplDemo.Model.Models;

namespace TplDemo.Services
{
    /// <summary>
    /// 需引用
    /// 仓储层接口+服务层接口
    /// </summary>
    public class SystemServices : ISystemServices
    {
        private readonly ISystemRepository system;

        public SystemServices(ISystemRepository system)
        {
            this.system = system;
        }

        public async Task<List<UserModule>> GetUserModule()
        {
            return await system.GetUserModule();
        }
    }
}
