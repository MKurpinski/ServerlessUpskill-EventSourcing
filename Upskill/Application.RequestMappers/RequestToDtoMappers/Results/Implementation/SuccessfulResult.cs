using System.Collections.Generic;
using System.Linq;

namespace Application.RequestMappers.RequestToDtoMappers.Results.Implementation
{
    public class SuccessfulResult<T> : IResult<T>
    {
        public bool Success => true;
        public T Value { get; }
        public IReadOnlyCollection<KeyValuePair<string, string>> Errors { get; }

        public SuccessfulResult(T value)
        {
            Value = value;
            Errors = Enumerable.Empty<KeyValuePair<string, string>>().ToList();
        }
    }
}