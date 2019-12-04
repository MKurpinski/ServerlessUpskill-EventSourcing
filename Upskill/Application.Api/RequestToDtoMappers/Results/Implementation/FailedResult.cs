using System.Collections.Generic;
using System.Linq;

namespace Application.Api.RequestToDtoMappers.Results.Implementation
{
    public class FailedResult<T> : IResult<T>
    {
        public bool Success => false;
        public T Value => default;
        public IReadOnlyCollection<KeyValuePair<string, string>> Errors { get; }

        public FailedResult(IEnumerable<KeyValuePair<string, string>> errors)
        {
            Errors = errors.ToList();
        }

        public FailedResult(string key, string message): this(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(key, message)})
        {
        }
    }
}