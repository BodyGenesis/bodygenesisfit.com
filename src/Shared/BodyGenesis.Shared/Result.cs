namespace BodyGenesis.Shared
{
    public class Result
    {
        protected Result()
        {
            HasError = false;
            Message = string.Empty;
        }

        protected Result(bool hasError, string message)
        {
            HasError = hasError;
            Message = message;
        }

        public static Result Error(string message)
        {
            return new Result(true, message);
        }

        public static Result Success()
        {
            return new Result();
        }

        public bool HasError { get; }
        public string Message { get; }
    }
}
