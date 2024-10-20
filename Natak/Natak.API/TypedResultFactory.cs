﻿using Natak.Domain;
using Natak.Infrastructure.ApiResponses;

namespace Natak.API;

public static class TypedResultFactory
{
    public static IResult Ok<TValue>(Result<TValue> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : BuildErrorResult(result.Error);
    }

    public static IResult Ok(Result result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return result.IsSuccess
            ? TypedResults.Ok(result)
            : BuildErrorResult(result.Error);
    }

    public static IResult NoContent<TValue>(Result<TValue> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : BuildErrorResult(result.Error);
    }

    public static IResult NoContent(Result result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : BuildErrorResult(result.Error);
    }

    private static IResult BuildErrorResult(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return TypedResults.Json(
            BuildErrorResponse(error),
            statusCode: (int)error.StatusCode);
    }

    private static ErrorResponse BuildErrorResponse(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return new ErrorResponse
        {
            StatusCode = (int)error.StatusCode,
            Message = error.Message,
            Type = error.Type
        };
    }
}
