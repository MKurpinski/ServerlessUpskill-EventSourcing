namespace Application.Api.Events.Internal
{
    public class ApplicationSavedInternalFunctionEvent : IInternalFunctionEvent
    {
        public ApplicationSavedInternalFunctionEvent(DataStorage.Model.Application application)
        {
            Application = application;
        }

        public DataStorage.Model.Application Application { get; }
    }
}
