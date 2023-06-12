using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ProductSales.Middleware;
public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task<Task> HandleExceptionAsync(HttpContext context, Exception exception)
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