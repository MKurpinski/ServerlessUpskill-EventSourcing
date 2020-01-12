using System.Collections.Generic;

namespace Upskill.Results
{
    public interface IMessageResult : IResult
    { 
        IReadOnlyCollection<KeyValuePair<string, string>> Errors { get; }
    }
}
