using System.Diagnostics.CodeAnalysis;

namespace FutSpect.Shared.Models.Result;

public record Result
{
    public bool IsSuccess { get; init; }
    public string Message { get; init; } = string.Empty;

    public static Result Fail(string message) =>
        new()
        {
            IsSuccess = false,
            Message = message
        };

    public static Result Success() =>
        new()
        {
            IsSuccess = true
        };
}

public record Result<T>
{
    [MemberNotNullWhen(returnValue: true, nameof(IsSuccess))]
    public T? Value { get; init; }

    public bool IsSuccess { get; init; }
    public string Message { get; init; } = string.Empty;

    public static Result<T> Fail(string message) =>
        new()
        {
            IsSuccess = false,
            Message = message,
            Value = default
        };

    public static Result<T> Success(T value) =>
        new()
        {
            IsSuccess = true,
            Value = value
        };
}