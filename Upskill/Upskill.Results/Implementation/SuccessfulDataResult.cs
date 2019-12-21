using System.Collections.Generic;
using System.Linq;

namespace Upskill.Results.Implementation
{
    public class SuccessfulDataResult<T> : SuccessfulResult, IDataResult<T>
    {
        public T Value { get; }
        public IReadOnlyCollection<KeyValuePair<string, string>> Errors { get; }

        public SuccessfulDataResult(T value)
        {
            Value = value;
            Errors = Enumerable.Empty<KeyValuePair<string, string>>().ToList();
        }
    }
}