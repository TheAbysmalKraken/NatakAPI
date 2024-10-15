using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Natak.Infrastructure.ApiResponses;

namespace Natak.Infrastructure.Middleware;

public sealed class ApiExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ApiExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(
        HttpContext context,
        IServiceProvider serviceProvider)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Exception occured: {ExceptionMessage}", e.Message);

            var errorResponse = new ErrorResponse()
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "An error occured while processing your request.",
                Type = "Natak.InternalServerError"
            };

            context.Response.StatusCode = errorResponse.StatusCode;
            context.Response.ContentType = "application/json";
            
            var responseString = JsonSerializer.Serialize(errorResponse);

            await context.Response.WriteAsync(responseString);
        }
    }
}