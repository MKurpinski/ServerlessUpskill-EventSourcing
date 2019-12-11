using System.Collections.Generic;

namespace Application.Api.Events.Internal
{
    public class ApplicationSaveFailedEvent : IEvent
    {
        public IReadOnlyCollection<KeyValuePair<string, string>> Errors { get; }

        public ApplicationSaveFailedEvent(IReadOnlyCollection<KeyValuePair<string, string>> errors)
        {
            Errors = errors;
        }
    }
}
