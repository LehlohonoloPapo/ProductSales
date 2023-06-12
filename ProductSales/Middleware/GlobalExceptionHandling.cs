using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
namespace ProductSales.Middleware;
using Serilog;
public class GlobalExceptionHandling
{
   
    private readonly RequestDelegate _next;

    public GlobalExceptionHandling( RequestDelegate next)
    {
        
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            Log.Logger.Information(context?.Request?.Path.Value.ToString(), context.Request.Cookies);
            await _next(context);
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task<Task> HandleExceptionAsync(HttpContext context, Exception? exception)
    {   
        // Perform any necessary error handling, logging, or response customization here
        // For example, you can set the response status code, return a JSON error response, etc.
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";
        var errorResponse = new
        {
            Message = "An error occurred while processing your request.",
            ExceptionMessage = exception.Message,
            StackTrace = exception.StackTrace
            // Additional properties as needed
        };
        var jsonResponse = JsonConvert.SerializeObject(errorResponse);
        return context.Response.WriteAsync(jsonResponse);
    }
}