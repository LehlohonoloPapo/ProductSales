using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ProductCoreLibrary.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    // private readonly ILogger _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
        // _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            // _logger.LogError(e, e.Message);
            Debug.WriteLine(e);
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
        }
    }
}