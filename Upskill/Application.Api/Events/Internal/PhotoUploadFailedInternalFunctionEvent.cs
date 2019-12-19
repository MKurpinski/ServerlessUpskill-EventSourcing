using System.Collections.Generic;

namespace Application.Api.Events.Internal
{
    public class PhotoUploadFailedInternalFunctionEvent : IInternalFunctionEvent
    {
        public IReadOnlyCollection<KeyValuePair<string, string>> Errors { get; }

        public PhotoUploadFailedInternalFunctionEvent(IReadOnlyCollection<KeyValuePair<string, string>> errors)
        {
            Errors = errors;
        }
    }
}
