namespace BodyGenesis.Shared
{
    public class Maybe<T>
        where T : class
    {
        private Maybe()
        { }

        private  Maybe(T value)
        {
            Value = value;
        }

        public static Maybe<T> None { get; } = new Maybe<T>(null);

        public static Maybe<T> From(T value)
        {
            return (value == null) ? None : new Maybe<T>(value);
        }

        public bool HasValue => (Value != null);
        public T Value { get; }
    }
}
