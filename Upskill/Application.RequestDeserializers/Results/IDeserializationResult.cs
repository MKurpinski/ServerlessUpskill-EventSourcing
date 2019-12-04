namespace Application.RequestDeserializers.Results
{
    public interface IDeserializationResult<T>
    { 
        bool Success { get; } 
        T Value { get; }
    }
}
