using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TplDemo.Model.Models;
using TplDemo.Model.ViewModels;

namespace TplDemo.IServices
{
    public interface ISystemServices
    {
        Task<List<UserModule>> GetUserModule();
        Task<List<UserModuleVM>> GetUserModuleVM();
    }
}
