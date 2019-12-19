namespace Application.Api.Events.Internal
{
    public class ApplicationSavedInternalFunctionEvent : IInternalFunctionEvent
    {
        public ApplicationSavedInternalFunctionEvent(DataStorage.Models.Application application)
        {
            Application = application;
        }

        public DataStorage.Models.Application Application { get; }
    }
}
