namespace WoM_WebApi.Configuration;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Log;

//this class records all security events (permission denied, login failed, etc.)
public class SecurityAuditFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {

        // This line actually runs the Controller method!
        ActionExecutedContext resultContext = await next();

        // We check what happened. Did the controller return an error?

        if (resultContext.Result is ObjectResult objectResult)
        {
            // check for 403 Forbidden or 401 Unauthorized
            if (objectResult.StatusCode == 403 || objectResult.StatusCode == 401)
            {
                var user = context.HttpContext.User.Identity?.Name ?? "Anonymous";
                var path = context.HttpContext.Request.Path;
                var method = context.HttpContext.Request.Method;

                // AUTOMATICALLY LOG THE SECURITY EVENT
                ActivityLog.Instance.Log(
                    "Access Denied",
                    $"User received {objectResult.StatusCode} on {method} {path}",
                    user
                );
            }
        }
        else if (resultContext.Result is StatusCodeResult statusResult)
        {
            // Sometimes controllers just return StatusCode(403) without an object
            if (statusResult.StatusCode == 403 || statusResult.StatusCode == 401)
            {
                var user = context.HttpContext.User.Identity?.Name ?? "Anonymous";
                var path = context.HttpContext.Request.Path;

                ActivityLog.Instance.Log(
                    "Access Denied",
                    $"User blocked at {path}",
                    user
                );
            }
        }
    }
}