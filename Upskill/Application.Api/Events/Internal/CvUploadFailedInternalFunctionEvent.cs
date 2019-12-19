using System.Collections.Generic;

namespace Application.Api.Events.Internal
{
    public class CvUploadFailedInternalFunctionEvent : IInternalFunctionEvent
    {
        public IReadOnlyCollection<KeyValuePair<string, string>> Errors { get; }

        public CvUploadFailedInternalFunctionEvent(IReadOnlyCollection<KeyValuePair<string, string>> errors)
        {
            Errors = errors;
        }
    }
}
