using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using Natak.Domain;

namespace Natak.Core.Abstractions.Behaviours;

public sealed class LoggingBehaviour<TRequest, TResponse>(
    ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var result = await next();

        if (result is not Result { IsFailure: true } failureResult)
        {
            return result;
        }
        
        var statusCode = failureResult.Error.StatusCode;
            
        if (statusCode == HttpStatusCode.InternalServerError)
        {
            logger.LogError(
                "{Request} failed: {Error}",
                typeof(TRequest).Name,
                failureResult.Error);
        }
        else
        {
            logger.LogWarning(
                "{Request} failed: {Error}",
                typeof(TRequest).Name,
                failureResult.Error);
        }

        return result;
    }
}