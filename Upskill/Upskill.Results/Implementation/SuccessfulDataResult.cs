using System.Collections.Generic;
using System.Linq;

namespace Upskill.Results.Implementation
{
    public class SuccessfulDataResult<T> : SuccessfulMessageResult, IDataResult<T>
    {
        public T Value { get; }

        public SuccessfulDataResult(T value)
        {
            Value = value;
        }
    }
}