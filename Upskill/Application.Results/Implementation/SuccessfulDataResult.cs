using System.Collections.Generic;
using System.Linq;

namespace Application.Results.Implementation
{
    public class SuccessfulDataResult<T> : IDataResult<T>
    {
        public bool Success => true;
        public T Value { get; }
        public IReadOnlyCollection<KeyValuePair<string, string>> Errors { get; }

        public SuccessfulDataResult(T value)
        {
            Value = value;
            Errors = Enumerable.Empty<KeyValuePair<string, string>>().ToList();
        }
    }
}