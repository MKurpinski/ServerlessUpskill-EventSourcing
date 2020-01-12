using System.Collections.Generic;

namespace Upskill.Results.Implementation
{
    public class FailedDataResult<T> : FailedMessageResult, IDataResult<T>
    {
        public T Value => default;

        public FailedDataResult(IEnumerable<KeyValuePair<string, string>> errors) : base(errors)
        {
        }

        public FailedDataResult(string key, string message): base(key, message)
        {
        }

        public FailedDataResult()
        {
        }
    }
}