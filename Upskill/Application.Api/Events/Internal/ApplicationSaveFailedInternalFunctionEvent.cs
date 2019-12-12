using System.Collections.Generic;

namespace Application.Api.Events.Internal
{
    public class ApplicationSaveFailedInternalFunctionEvent : IInternalFunctionEvent
    {
        public IReadOnlyCollection<KeyValuePair<string, string>> Errors { get; }

        public ApplicationSaveFailedInternalFunctionEvent(IReadOnlyCollection<KeyValuePair<string, string>> errors)
        {
            Errors = errors;
        }
    }
}
