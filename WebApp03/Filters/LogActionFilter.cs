using System.IO;
using System.Web.Mvc;

namespace WebApp03.Filters
{
    public class LogActionFilter: IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionName = filterContext.ActionDescriptor.ActionName;
            var ctrlName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var path = filterContext.HttpContext.Server.MapPath("~/log.txt");
            File.AppendAllText(path, $"將要執行{ctrlName}: {actionName}");
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var actionName = filterContext.ActionDescriptor.ActionName;
            var ctrlName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var path = filterContext.HttpContext.Server.MapPath("~/log.txt");
            File.AppendAllText(path, $"執行完畢{ctrlName}: {actionName}");
        }
    }
}