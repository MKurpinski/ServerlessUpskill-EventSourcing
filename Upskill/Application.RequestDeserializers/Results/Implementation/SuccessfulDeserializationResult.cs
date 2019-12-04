namespace Application.RequestDeserializers.Results.Implementation
{
    public class SuccessfulDeserializationResult<T> : IDeserializationResult<T>
    {
        public bool Success => true;
        public T Value { get; }

        public SuccessfulDeserializationResult(T value)
        {
            Value = value;
        }
    }
}