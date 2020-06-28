using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TplDemo.Model.Models;

namespace TplDemo.IServices
{
    public interface ISystemServices
    {
        Task<List<UserModule>> GetUserModule();
    }
}
