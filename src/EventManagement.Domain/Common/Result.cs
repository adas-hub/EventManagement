namespace EventManagement.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public List<Error> Errors { get; }

    public Error Error => Errors.FirstOrDefault() ?? Error.None;

    protected Result(bool isSuccess, List<Error> errors)
    {
        if (isSuccess && errors != null && errors.Any())
        {
            throw new InvalidOperationException("A successful result cannot have errors.");
        }

        if (!isSuccess && (errors == null || !errors.Any()))
        {
            throw new InvalidOperationException("A failed result must have at least one error.");
        }

        IsSuccess = isSuccess;
        Errors = errors ?? new List<Error>();
    }

    public static Result Success() => new(true, new List<Error>());

    public static Result Failure(Error error) => new(false, new List<Error> { error });

    public static Result Failure(List<Error> errors) => new(false, errors);

    public static implicit operator Result(List<Error> errors) => Failure(errors);
    public static implicit operator Result(Error error) => Failure(error);
}
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected Result(TValue value, bool isSuccess) : base(isSuccess, new List<Error>())
    {
        _value = value;
    }

    protected Result(TValue? value, bool isSuccess, List<Error> errors) : base(isSuccess, errors)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result cannot be accessed.");

    public static Result<TValue> Success(TValue value) => new(value, true);
    public static new Result<TValue> Failure(Error error) => new(default, false, new List<Error> { error });
    public static new Result<TValue> Failure(List<Error> errors) => new(default, false, errors);


    public static implicit operator Result<TValue>(TValue? value)
    {
        return value is not null
            ? Success(value)
            : Failure(Error.Failure("Error.NullValue", "The specified result value is null.")); ;
    }

    public static implicit operator Result<TValue>(Error error) => Failure(error);
    public static implicit operator Result<TValue>(List<Error> errors) => Failure(errors);
}