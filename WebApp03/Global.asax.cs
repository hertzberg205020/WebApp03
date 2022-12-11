using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApp03.Filters;

namespace WebApp03
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            
            Bootstrapper.Run();
            
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // AutofacConfig.Register();
            // 指定傳回JSON
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            
            // 註冊Filter
            // GlobalFilters.Filters.Add(new CheckLoginFilter());
            // GlobalFilters.Filters.Add(new LogActionFilter());
            // GlobalFilters.Filters.Add(new ExceptionFilter());
        }
    }
}
