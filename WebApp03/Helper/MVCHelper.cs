using System.Text;
using System.Web.Http.ModelBinding;


namespace WebApp03.Tool
{
    public static class MVCHelper
    {
        public static string GetValidMsg(ModelStateDictionary modelState)
        {
            StringBuilder stringBuilder = new StringBuilder();
            // 
            foreach (var prop in modelState.Keys)
            {
                // modelState[prop].Errors為屬性相關的錯誤訊息
                if (modelState[prop].Errors.Count <= 0)
                {
                    continue;
                }
                
                stringBuilder.Append("Property is invalid: ")
                    .Append(prop)
                    .Append(":");
                // 遍歷每一個錯誤訊息(一個屬性可能不只一個錯誤訊息)
                foreach (var error in modelState[prop].Errors)
                {
                    stringBuilder.Append(error.ErrorMessage);
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}