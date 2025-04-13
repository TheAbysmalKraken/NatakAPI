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
        var response = await next();

        if (response is not Result { IsFailure: true } failureResult)
        {
            if (response is IValueResult successResult)
            {
                logger.LogInformation("{RequestType} completed successfully. Request: {Request}. Response: {@Response}.",
                    typeof(TRequest).Name,
                    request,
                    successResult.GetValue());
            }
            else
            {
                logger.LogInformation("{RequestType} completed successfully. Request: {Request}. Response: {@Response}.",
                    typeof(TRequest).Name,
                    request,
                    response);
            }
            
            return response;
        }
        
        var statusCode = failureResult.Error.StatusCode;
            
        if (statusCode == HttpStatusCode.InternalServerError)
        {
            logger.LogError(
                "{RequestType} failed. Request: {Request}. Error: {Error}.",
                typeof(TRequest).Name,
                request,
                failureResult.Error);
        }
        else
        {
            logger.LogWarning(
                "{RequestType} failed. Request: {Request}. Error: {Error}.",
                typeof(TRequest).Name,
                request,
                failureResult.Error);
        }

        return response;
    }
}