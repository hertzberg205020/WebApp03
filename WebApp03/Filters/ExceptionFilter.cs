using System;
using System.IO;
using System.Web.Mvc;

namespace WebApp03.Filters
{
    public class ExceptionFilter: IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var path = filterContext.HttpContext.Server.MapPath("~/err.txt");
            Exception ex = filterContext.Exception;
            
            filterContext.ExceptionHandled = true;  // 若將filterContext.ExceptionHandled設置為true，其他的IExceptionFilter將不會被執行，
            
            File.AppendAllText(path, $"{ex}\n");
            filterContext.Result = new ContentResult { Content = $"error"};
        }
    }
}