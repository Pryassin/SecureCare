namespace CMS.Domain.Common;

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("General.Null", "Value cannot be null");
    public static implicit operator string(Error error) => error.Code;
}
public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);

    // Allows you to return "Errors.User.InvalidRole" directly
    public static implicit operator Result(Error error) => Failure(error);
}

// 2. The Generic Result (for Create/Queries)
public class Result<T> : Result
{
    private readonly T? _value;

    // If it's a success, we expect a value.
    public T Value => IsSuccess 
        ? _value! 
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    protected internal Result(T? value, bool isSuccess, Error error) 
        : base(isSuccess, error)
    {
        _value = value;
    }

    public new static Result<T> Success(T value) => new(value, true, Error.None);
    public new static Result<T> Failure(Error error) => new(default, false, error);

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);
}