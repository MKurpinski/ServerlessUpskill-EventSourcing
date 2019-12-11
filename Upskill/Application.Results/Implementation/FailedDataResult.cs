using System.Collections.Generic;
using System.Linq;

namespace Application.Results.Implementation
{
    public class FailedDataResult<T> : IDataResult<T>
    {
        public bool Success => false;
        public T Value => default;
        public IReadOnlyCollection<KeyValuePair<string, string>> Errors { get; }

        public FailedDataResult(IEnumerable<KeyValuePair<string, string>> errors)
        {
            Errors = errors.ToList();
        }

        public FailedDataResult(string key, string message): this(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(key, message)})
        {
        }

        public FailedDataResult()
        {
        }
    }
}