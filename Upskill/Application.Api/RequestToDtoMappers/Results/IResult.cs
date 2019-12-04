using System.Collections.Generic;

namespace Application.Api.RequestToDtoMappers.Results
{
    public interface IResult<T>
    { 
        bool Success { get; } 
        T Value { get; }
        IReadOnlyCollection<KeyValuePair<string, string>> Errors { get; }
    }
}
