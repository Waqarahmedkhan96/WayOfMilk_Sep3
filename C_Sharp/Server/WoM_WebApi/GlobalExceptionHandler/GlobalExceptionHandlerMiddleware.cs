// File: Server/WoM_WebApi/GlobalExceptionHandler/GlobalExceptionHandlerMiddleware.cs
using Microsoft.AspNetCore.Http;

namespace WoM_WebApi.GlobalExceptionHandler;

// Middleware: catch all exceptions
public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            // go to next middleware / controller
            await next(context);
        }
        catch (NotFoundException ex)
        {
            Console.WriteLine(ex);
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync(ex.Message);
        }
        catch (ValidationException ex)
        {
            Console.WriteLine(ex);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("An unexpected error occurred.");
        }
    }
}
