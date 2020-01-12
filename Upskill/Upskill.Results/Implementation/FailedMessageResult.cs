using System.Collections.Generic;
using System.Linq;

namespace Upskill.Results.Implementation
{
    public class FailedMessageResult : FailedResult, IMessageResult
    {
        public FailedMessageResult()
        {
            Errors = Enumerable.Empty<KeyValuePair<string, string>>().ToList();
        }
        public FailedMessageResult(IEnumerable<KeyValuePair<string, string>> errors)
        {
            Errors = errors.ToList();
        }

        public FailedMessageResult(string key, string message) : this(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(key, message) })
        {
        }

        public IReadOnlyCollection<KeyValuePair<string, string>> Errors { get; }
    }
}