using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;

namespace WebApp03;

public class Bootstrapper
{
    public static void Run()  
    {  
        //Configure AutoFac  
        AutofacConfig.Initialize(GlobalConfiguration.Configuration);  
    }  
}