using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Upskill.Results;
using Upskill.Results.Implementation;

namespace Upskill.Telemetry.Facades
{
    public class ActivityFacade : IActivityFacade
    {
        private const string CORRELATION_ID_TAG = "customTag_correlationId";

        public void SetCorrelationId(string correlationId)
        {
            Activity.Current?.AddTag(CORRELATION_ID_TAG, correlationId);
        }

        public IDataResult<string> GetCorrelationId()
        {
            var correlationIdTag = Activity.Current?.Tags?.FirstOrDefault(z => z.Key == CORRELATION_ID_TAG);

            if (!correlationIdTag.HasValue || correlationIdTag.Value.Equals(default(KeyValuePair<string, string>)))
            {
                return new FailedDataResult<string>();
            }

            return new SuccessfulDataResult<string>(correlationIdTag.Value.Value);
        }
    }
}