using System;

namespace BodyGenesis.Shared
{
    public class Result<T> : Result
        where T : class
    {
        protected Result(bool hasError, string message)
            : base(hasError, message)
        { }

        protected Result(T value)
            : base(false, string.Empty)
        {
            Value = value;
        }

        public static new Result<T> Error(string message)
        {
            return new Result<T>(true, message);
        }

        public static Result<T> Success(T value)
        {
            value = value ?? throw new ArgumentNullException(nameof(value));

            return new Result<T>(value);
        }

        public T Value { get; }
    }
}
