using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TplDemo.IRepository;
using TplDemo.IServices;
using TplDemo.Model.Models;
using TplDemo.Model.ViewModels;

namespace TplDemo.Services
{
    /// <summary>
    /// 需引用
    /// 仓储层接口+服务层接口
    /// </summary>
    public class SystemServices : ISystemServices
    {
        private readonly ISystemRepository system;
        private readonly IMapper mapper;

        public SystemServices(ISystemRepository system, IMapper mapper)
        {
            this.system = system;
            this.mapper = mapper;
        }

        public async Task<List<UserModule>> GetUserModule()
        {
            return await system.GetUserModule();
        }

        public async Task<List<UserModuleVM>> GetUserModuleVM()
        {
            return mapper.Map<List<UserModuleVM>>(await system.GetUserModule());
        }
    }
}
