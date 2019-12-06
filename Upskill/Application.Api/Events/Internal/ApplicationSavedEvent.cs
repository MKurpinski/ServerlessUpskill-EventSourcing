namespace Application.Api.Events.Internal
{
    public class ApplicationSavedEvent : IEvent
    {
        public ApplicationSavedEvent(DataStorage.Model.Application application)
        {
            Application = application;
        }

        public DataStorage.Model.Application Application { get; }
    }
}
