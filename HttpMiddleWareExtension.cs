using System.Net;
using System.Text.Json;
using MadSoul.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MadSoul.AspCommon;

public class HttpExceptionMiddleware(RequestDelegate next , ILogger<HttpExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DtoException ex)
        {
            if (ex.InternalException is not null)
                logger.LogError(ex , "Exception handled by HttpExceptionMiddleware");
            
            await HandleExceptionAsync(context, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex , "Generic Exception handled by HttpExceptionMiddleware");
            await HandleExceptionAsync(context, "خطایی رخ داده است");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, string errorMessage)
    {
        // Set response details
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // Create a standardized error response
        var errorResponse = new DtoResponse()
        {
            Code = 200,
            Errors = [errorMessage]
        };
        var jsonResult = JsonSerializer.Serialize(errorResponse);

        return context.Response.WriteAsync(jsonResult);
    }
    
}