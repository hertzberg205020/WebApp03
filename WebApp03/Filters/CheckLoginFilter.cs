using System.Web.Mvc;

namespace WebApp03.Filters
{
    public class CheckLoginFilter: IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            // 當前訪問的Controller的名字
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            // 當前訪問的Action的名字
            string actionName = filterContext.ActionDescriptor.ActionName;
            // 獲得要執行的controller的類型
            // var controllerType = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType;
            if (controllerName != "Login" || (actionName != "Index" && actionName != "Login"))
            {
                // 檢測登入狀態
                if (filterContext.HttpContext.Session["UserName"] == null)
                {
                    // 若在filter中給filterContext.Result賦值，那麼將不再繼續執行後續的filter與Action
                    // 而是以filterContext.Result的值作為執行結果，返回給客戶端
                    // 阻止Action執行
                    filterContext.Result = new ContentResult { Content = "未登入狀態"};
                    // filterContext.Result = new RedirectResult("/Login/Index");
                    // 如果執行的是filterContext.HttpContext.Response.Redirect("/Home/Index"); 那麼目標Action還是會執行
                }
            }
        }
    }
}