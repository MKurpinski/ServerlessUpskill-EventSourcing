using System.Collections.Generic;
using System.Linq;

namespace Upskill.Results.Implementation
{
    public class SuccessfulMessageResult : SuccessfulResult, IMessageResult
    {
        public IReadOnlyCollection<KeyValuePair<string, string>> Errors => Enumerable.Empty<KeyValuePair<string, string>>().ToList();
    }
}