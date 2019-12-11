using System.Collections.Generic;

namespace Application.Results
{
    public interface IDataResult<T> : IResult
    {
        T Value { get; }

        IReadOnlyCollection<KeyValuePair<string, string>> Errors { get; }
    }
}
