using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.Model.Models;
using TplDemo.Model.ViewModels;

namespace TplDemo.Extensions.ServiceExtensions.AutoMap
{
    public class CustomProfile : Profile
    {

        public CustomProfile()
        {
            CreateMap<UserModule, UserModuleVM>().ForMember(d => d.ModuleName, o => o.MapFrom(s => s.Name));
            CreateMap<UserModuleVM, UserModule>().ForMember(d => d.Name, o => o.MapFrom(s => s.ModuleName));
        }

    }
}
