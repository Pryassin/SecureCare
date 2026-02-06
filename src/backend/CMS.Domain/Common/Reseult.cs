namespace CMS.Domain.Common;

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("General.Null", "Value cannot be null");
}

public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public Error Error { get; } // Changed from string to Error
    public bool IsFailure => !IsSuccess;
    
    private Result(bool isSuccess, T value, Error error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }
    
    public static Result<T> Success(T value) => 
        new Result<T>(true, value, Error.None);
    
    public static Result<T> Failure(Error error) => 
        new Result<T>(false, default, error);
    
    // Implicit conversion from T to Result<T>
    public static implicit operator Result<T>(T value) => 
        value is not null ? Success(value) : Failure(Error.NullValue);
    
    // Implicit conversion from Error to Result<T>
    public static implicit operator Result<T>(Error error) => Failure(error);
}