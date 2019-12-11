using System.Collections.Generic;

namespace Application.Api.Events.Internal
{
    public class CvUploadFailedEvent : IEvent
    {
        public IReadOnlyCollection<KeyValuePair<string, string>> Errors { get; }

        public CvUploadFailedEvent(IReadOnlyCollection<KeyValuePair<string, string>> errors)
        {
            Errors = errors;
        }
    }
}
