using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TplDemo.IRepository.BASE;
using TplDemo.Model.Models;

namespace TplDemo.IRepository
{
    /// <summary>
    /// 引用Model层
    /// </summary>
    public interface ISystemRepository
    {
        Task<List<UserModule>> GetUserModule();
    }
}
