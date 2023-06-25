namespace CT.Examples.AspNetCoreWithAngular.Models;

public class OperationResult<T>
{
    public T? Data { get; }
    public OperationResultStatus Status { get; }
    public string? ErrorMessage { get; set; }

    public bool Success => Status == OperationResultStatus.Success;

    public OperationResult(T data)
    {
        Data = data;
        Status = OperationResultStatus.Success;
    }

    public OperationResult(OperationResultStatus status, string errorMessage)
    {
        Status = status;
        ErrorMessage = errorMessage;
    }

    public static implicit operator OperationResult<T>(T value)
    {
        return new OperationResult<T>(value);
    }

    public static implicit operator OperationResult<T>(OperationResultStatus status)
    {
        return new OperationResult<T>(status, "");
    }
}

public enum OperationResultStatus
{
    Success,
    BadRequest,
    NotFound,
    Forbidden,
    Conflict,
    Error
}