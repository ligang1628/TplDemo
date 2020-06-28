## 仓储模式
> 1、新建 IRepository:Model
> 2、新建 Repository 引用 && 继承 IRepository
> 3、新建 IService 引用 IRepository
> 4、新建 Service 引用 IService && IRepository,继承 IService,在服务层中注入 IRepository
> 5、API 层引入 IRepository && IService
> 6、在 API 层安装 NuGet 包 
>> Autofac.Extensions.DependencyInjection
>> Autofac.Extras.DynamicProxy
> 7、在 Startup 中新增

```
public void ConfigureContainer(ContainerBuilder builder)
{
	builder.RegisterModule(new AutofacModuleRegister());
}
```

> 8、在 Program 类 CreateHostBuilder 方法加入

```
.UseServiceProviderFactory(new AutofacServiceProviderFactory())
```

> 9、新建类 AutofacModuleRegister
>> 将 Service 层和 Repository 层注入到程序集

```
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

        // 获取 Serivces.dll 程序集服务，并注册
        var assemblysServices = Assembly.LoadFrom(servicesDll);
        builder.RegisterAssemblyTypes(assemblysServices)
            .AsImplementedInterfaces()
            .InstancePerDependency();

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

    }
}
```

