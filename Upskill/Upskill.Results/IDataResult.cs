namespace Upskill.Results
{
    public interface IDataResult<T> : IMessageResult
    {
        T Value { get; }
    }
}
