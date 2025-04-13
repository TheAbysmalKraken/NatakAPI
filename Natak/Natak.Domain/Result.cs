using System.Diagnostics.CodeAnalysis;

namespace Natak.Domain;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
}

public class Result<TValue> : Result, IValueResult
{
    private readonly TValue? value;

    protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        this.value = value;
    }

    [NotNull]
    public TValue Value => IsSuccess ? value! : throw new InvalidOperationException("No value present");
    
    public object GetValue()
    {
        if (IsSuccess)
        {
            return value!;
        }

        throw new InvalidOperationException("No value present");
    }
}