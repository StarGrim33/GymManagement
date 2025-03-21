﻿namespace GymManagement.Domain.Abstractions;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        switch (isSuccess)
        {
            case true when error != Error.None:
                throw new InvalidOperationException();

            case false when error == Error.None:
                throw new InvalidOperationException();

            default:
                IsSuccess = isSuccess;
                Error = error;
                break;
        }
    }

    public Error Error { get; }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public static Result<TValue> Failure<TValue>(Error error, string code, string description) => Failure<TValue>(new Error(code, description));

    public static Result<TValue> Create<TValue>(TValue? value) => 
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error) 
        : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed");

    public static implicit operator Result<TValue>(TValue value) => Create(value);
}