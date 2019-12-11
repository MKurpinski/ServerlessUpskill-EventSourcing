using System.Collections.Generic;

namespace Application.Api.Events.Internal
{
    public class PhotoUploadFailedEvent : IEvent
    {
        public IReadOnlyCollection<KeyValuePair<string, string>> Errors { get; }

        public PhotoUploadFailedEvent(IReadOnlyCollection<KeyValuePair<string, string>> errors)
        {
            Errors = errors;
        }
    }
}
