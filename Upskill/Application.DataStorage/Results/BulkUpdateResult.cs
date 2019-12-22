using System.Collections.Generic;
using System.Linq;
using Upskill.Results;

namespace Application.DataStorage.Results
{
    public class BulkUpdateResult<T> : IResult
    {
        public bool Success => !FailedUpdatedDocuments.Any();

        public IReadOnlyCollection<T> FailedUpdatedDocuments { get; set; }
        public IReadOnlyCollection<T> SuccessfulUpdatedDocuments { get; set; }

        public BulkUpdateResult(
            IReadOnlyCollection<T> successfulUpdatedDocuments, 
            IReadOnlyCollection<T> failedUpdatedDocuments)
        {
            FailedUpdatedDocuments = failedUpdatedDocuments;
            SuccessfulUpdatedDocuments = successfulUpdatedDocuments;
        }
    }
}
