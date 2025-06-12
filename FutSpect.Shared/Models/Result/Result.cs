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

    public static Result Ok() =>
        new()
        {
            IsSuccess = true
        };
}

public record Result<T> where T : notnull
{
    public T? Value { get; init; }

    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess { get; init; }

    public string Message { get; init; } = string.Empty;

    public static Result<T> Ok(T value) =>
        new()
        {
            IsSuccess = true,
            Value = value
        };

    public static Result<T> Fail(string message) =>
        new()
        {
            IsSuccess = false,
            Message = message,
            Value = default
        };
}