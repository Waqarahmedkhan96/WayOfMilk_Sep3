using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RepositoryContracts.ExceptionHandling; 

namespace WebAPI.GlobalExceptionHandler;

public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    // Handles all exceptions in one place for the entire app
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context); // run next middleware or controller
        }
        catch (NotFoundException ex)
        {
            Console.WriteLine(ex); // log error
            context.Response.StatusCode = StatusCodes.Status404NotFound; // 404 if not found
            await context.Response.WriteAsync(ex.Message); // send message to client
        }
        catch (ValidationException ex)
        {
            Console.WriteLine(ex); // log error
            context.Response.StatusCode = StatusCodes.Status400BadRequest; // 400 if bad input
            await context.Response.WriteAsync(ex.Message); // send message to client
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex); // log unexpected error
            context.Response.StatusCode = StatusCodes.Status500InternalServerError; // 500 for server errors
            await context.Response.WriteAsync(ex.Message); // send message to client
        }
    }
}
