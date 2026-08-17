namespace VSRSystemsBackend.Core.Common;

public class Result
{
    public bool IsSuccess { get; protected set; }
    public string Error { get; protected set; } = string.Empty;
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, string.Empty);
    public static Result Failure(string error) => new(false, error);
}

public class Result<T> : Result
{
    public T? Value { get; private set; }

    protected Result(T value, bool isSuccess, string error) : base(isSuccess, error)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(value, true, string.Empty);
    public static new Result<T> Failure(string error) => new(default!, false, error);
    
    public static implicit operator Result<T>(T value) => Success(value);
}