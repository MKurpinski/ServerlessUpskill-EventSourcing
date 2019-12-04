namespace Application.RequestDeserializers.Results.Implementation
{
    public class FailedDeserializationResult<T> : IDeserializationResult<T>
    {
        public bool Success => false;
        public T Value => default;
    }
}