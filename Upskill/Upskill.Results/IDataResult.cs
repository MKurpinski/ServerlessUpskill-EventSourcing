using System.Collections.Generic;

namespace Upskill.Results
{
    public interface IDataResult<T> : IResult
    {
        T Value { get; }

        IReadOnlyCollection<KeyValuePair<string, string>> Errors { get; }
    }
}
