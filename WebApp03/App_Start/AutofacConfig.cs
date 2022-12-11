using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using WebApp03.Models;
using WebApp03.Repository;
using WebApp03.Repository.Impl;
using WebApp03.Services;
using WebApp03.Services.Impl;

namespace WebApp03;

/// <summary>
/// 配置請參照以下網址
/// https://www.c-sharpcorner.com/article/using-autofac-with-web-api/
/// </summary>
public class AutofacConfig
{
    public static IContainer Container;  
  
    public static void Initialize(HttpConfiguration config)  
    {  
        Initialize(config, RegisterServices(new ContainerBuilder()));  
    }  
  
  
    public static void Initialize(HttpConfiguration config, IContainer container)  
    {  
        config.DependencyResolver = new AutofacWebApiDependencyResolver(container);  
    }    
  
    /// <summary>
    /// 生命週期請不要亂配
    /// https://toyo0103.github.io/2018/07/12/%E3%80%90Autofac%E3%80%91%E7%94%9F%E5%91%BD%E9%80%B1%E6%9C%9F/
    /// https://www.cnblogs.com/bluesummer/p/8875702.html
    /// https://www.cnblogs.com/masonblog/p/9563199.html
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    private static IContainer RegisterServices(ContainerBuilder builder)  
    {  
        //Register your Web API controllers.  
        builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

        // Registering Generic Interfaces and classes with Autofac
        // https://stackoverflow.com/questions/42943728/registering-generic-interfaces-and-classes-with-autofac
        // multiple Ttypes
        // https://stackoverflow.com/questions/54571968/how-to-register-generic-class-with-autofac-that-requires-parameters
        builder.RegisterGeneric(typeof(ProxyDao<,>))
            .As(typeof(IProxyDao<,>));

        builder.RegisterGeneric(typeof(ProxyService<,>))
            .As(typeof(IProxyService<,>));

        // builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
        //     .AsClosedTypesOf(typeof(ICoreDao<,>));
        
        builder.RegisterType<ExamDbContext>()  
            .As<DbContext>()  
            .InstancePerLifetimeScope();  
        
        builder.RegisterType<DbContextHolder>()  
            .As<IDbContextHolder>()
            .InstancePerLifetimeScope();
        
        // Data Access Layer可為單例
        builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .Where(t => t.Name.EndsWith("Dao")
                        && !t.Name.Contains("ProxyDao"))
            .AsImplementedInterfaces().SingleInstance();
        
        // Business Logic Layer為單例
        builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .Where(t => t.Name.EndsWith("Service")
                        && !t.Name.Contains("ProxyService"))
            .AsImplementedInterfaces().SingleInstance();

        // 這裡必須這樣指定，因為設計中採用代理模式，xxxDao皆有實現ICore<,>泛型interface
        // autofac在注入時，會發生多個實例實現同一個interface，導致循環引用
        // 使用C#8.0 interface default可避免此情況
        
        // builder.RegisterType<EmpDao>().As<IEmpDao>();
        // builder.RegisterType<SubjectDao>().As<ISubjectDao>();
        // builder.RegisterType<ExamDao>().As<IExamDao>();
        
        // builder.RegisterGeneric(typeof(GenericRepository<>))  
        //     .As(typeof(IGenericRepository<>))  
        //     .InstancePerRequest();  
  
        //Set the dependency resolver to be Autofac.  
        Container = builder.Build();

        return Container;  
    }
    
    // public static void Register()
    // {
    //     // 第一步，建立ContainerBuilder
    //     // var builder = new ContainerBuilder();
    //
    //     // 開始 第二步，註冊service
    //
    //     // 註冊ConsoleLogger這個class為ILogger Service的Component
    //     // builder.RegisterType<EmpDao>().As<IEmpDao>();
    //
    //     // 如果寫法是：builder.RegisterType<ConsoleLogger>(); 
    //     // 那麼就是ConsoleLogger這個class為ConsoleLogger Service的Component
    //
    //     // 註冊自己實例化出來的物件
    //     // 上面是只註冊class type（因此，用到的時候是autofac幫你new出來），這邊是直接用這個object
    //     // var output = new StringWriter();
    //     // builder.RegisterInstance(output).As<TextWriter>();
    //
    //     // 用lambda 註冊會更有彈性，因為傳進來的c是container的instance，因此可以用c來做一些複雜的東西。
    //     // builder.Register(c => new EmpService()).As<IEmpService>();
    //
    //     // 假設不想要一個一個註冊，可以用scan的方式。
    //     // 下面scan一個assembly所有的type，當那個type的名字最後結尾是Repository的時候，
    //     // 把它註冊的service設為這個class的interface
    //     // builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
    //     //     .Where(t => t.Name.EndsWith("Dao"))
    //     //     .AsImplementedInterfaces();
    //     
    //     // 下面這種很暴力，AppDomain.CurrentDomain.GetAssemblies()加載專案中所有用到的依賴程序集(組件)
    //     // builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
    //     //     .Where(t => t.Name.EndsWith("Dao"))
    //     //     .AsImplementedInterfaces();
    
    //     // builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
    //     //        .Where(t => t.Name.EndsWith("Repository"))
    //     //        .AsImplementedInterfaces()
    //     //        .InstancePerRequest();
    //
    //     // 加載特定的Assemble
    //     // var assemblyType = typeof(MyCustomAssemblyType).GetTypeInfo();
    //     // builder.RegisterAssemblyTypes(assemblyType.Assembly)
    //     //        .Where(t => t.Name.EndsWith("Repository"))
    //     //        .AsImplementedInterfaces()
    //     //        .InstancePerRequest();
    //
    //     // var webAssembly = Assembly.GetExecutingAssembly();
    //     // var repoAssembly = Assembly.GetAssembly(typeof(SampleRepository)); // Assuming SampleRepository is within the Repository project
    //     // builder.RegisterAssemblyTypes(webAssembly, repoAssembly)
    //     //        .AsImplementedInterfaces();
    //
    //     
    //     // builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
    //     //     .Where(t => t.Name.EndsWith("Dao"))
    //     //     .AsImplementedInterfaces();
    //     //
    //     // builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
    //     //     .Where(t => t.Name.EndsWith("Service"))
    //     //     .AsImplementedInterfaces();
    //     //
    //     // builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
    //     //結束 第二步
    //
    //     // 第三步，註冊都完成了，建立自己的container
    //     // var container = builder.Build();
    //
    //     // 第四部，從container取得對應的component。
    //     // 這邊用using包起來，因為出了這個scope，一切Resolve出來的都會被釋放掉。
    //     // 這部份在我們整個系列碰到並不多，因為不建議自己每一個這樣取出來，
    //     // 而是用深度整合的方式來讓一切像自動發生。
    //     // 詳細之後就會比較清楚
    //     // using(var scope = container.BeginLifetimeScope())
    //     // {
    //     //     var reader = container.Resolve<IEmpDao>();
    //     // }
    //     // var resolver = new AutofacWebApiDependencyResolver(container);
    //
    //     // DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
    //     DependencyResolver.SetResolver(new AutofacWebApiDependencyResolver(container));
    // }
}