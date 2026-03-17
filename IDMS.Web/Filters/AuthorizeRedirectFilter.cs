using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IDMS.Web.Filters
{
    public class AuthorizeRedirectFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is UnauthorizedAccessException)
            {
                context.ExceptionHandled = true;
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
            return Task.CompletedTask;
        }
    }
}