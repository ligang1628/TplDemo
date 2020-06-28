using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.IRepository;
using TplDemo.Model.Context;
using TplDemo.Model.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TplDemo.IRepository.BASE;

namespace TplDemo.Repository
{
    /// <summary>
    /// 引用仓储层
    /// </summary>
    public class SystemRepository : ISystemRepository
    {
        private readonly MSSQLContext context;

        public SystemRepository(MSSQLContext context)
        {
            this.context = context;
        }

        public async Task<List<UserModule>> GetUserModule()
        {
            return await context.UserModule.ToListAsync();
        }
    }
}
