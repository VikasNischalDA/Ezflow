using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LeadManagementSystem.MVC
{
    public class AuthorizeTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Session.GetString("AuthToken");

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new RedirectResult("~/Login/Index");
            }
        }
    }
}
