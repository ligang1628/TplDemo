﻿using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using TplDemo.Comment;
using TplDemo.Extensions.AOP;

namespace TplDemo.Extensions.ServiceExtensions.AutofacModule
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;

            #region 带有接口层的服务注册

            var servicesDll = Path.Combine(basePath, "TplDemo.Services.dll");
            var repositoryDll = Path.Combine(basePath, "TplDemo.Repository.dll");
            if (!File.Exists(servicesDll) && File.Exists(repositoryDll))
            {
                var msg = "Repository.dll和Services.dll丢失，重新生成解决方案即可解决";
                throw new Exception(msg);
            }

            // AOP 开关，如果想要打开指定的功能，只需要在 appsettigns.json 对应对应 true 就行。
            var cacheType = new List<Type>();
            //if (Appsettings.App(new string[] { "AppSettings", "RedisCachingAOP", "Enabled" }).ObjToBool())
            //{
            //    builder.RegisterType<TplDemoRedisCacheAOP>();
            //    cacheType.Add(typeof(BlogRedisCacheAOP));
            //}
            if (bool.Parse(Appsettings.App(new string[] { "AppSettings", "MemoryCachingAOP", "Enabled" })))
            {
                builder.RegisterType<TplDemoCacheAOP>();
                cacheType.Add(typeof(TplDemoCacheAOP));
            }
            //if (Appsettings.App(new string[] { "AppSettings", "TranAOP", "Enabled" }).ObjToBool())
            //{
            //    builder.RegisterType<BlogTranAOP>();
            //    cacheType.Add(typeof(BlogTranAOP));
            //}
            if (bool.Parse(Appsettings.App(new string[] { "AppSettings", "LogAOP", "Enabled" })))
            {
                builder.RegisterType<TplDemoLogAOP>();
                cacheType.Add(typeof(TplDemoLogAOP));
            }

            // 获取 Serivces.dll 程序集服务，并注册
            var assemblysServices = Assembly.LoadFrom(servicesDll);
            builder.RegisterAssemblyTypes(assemblysServices)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                //引用Autofac.Extras.DynamicProxy;
                .EnableInterfaceInterceptors()
                ////允许将拦截器服务的列表分配给注册。
                .InterceptedBy(cacheType.ToArray());

            // 获取 Repository.dll 程序集服务，并注册
            var assemblysRepository = Assembly.LoadFrom(repositoryDll);
            builder.RegisterAssemblyTypes(assemblysRepository)
                .AsImplementedInterfaces()
                .InstancePerDependency();

            #endregion

            #region 没有接口层的服务层注入

            //因为没有接口层，所以不能实现解耦，只能用 Load 方法。
            //注意如果使用没有接口的服务，并想对其使用 AOP 拦截，就必须设置为虚方法
            //var assemblysServicesNoInterfaces = Assembly.Load("Blog.Core.Services");
            //builder.RegisterAssemblyTypes(assemblysServicesNoInterfaces);

            #endregion

            #region 没有接口的单独类，启用class代理拦截

            //只能注入该类中的虚方法，且必须是public
            //这里仅仅是一个单独类无接口测试，不用过多追问
            //builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(Love)))
            //    .EnableClassInterceptors()
            //    .InterceptedBy(cacheType.ToArray());
            #endregion

            #region 单独注册一个含有接口的类，启用interface代理拦截

            //不用虚方法
            //builder.RegisterType<AopService>().As<IAopService>()
            //   .AsImplementedInterfaces()
            //   .EnableInterfaceInterceptors()
            //   .InterceptedBy(typeof(BlogCacheAOP));
            #endregion

            //base.Load(builder);
        }
    }
}
