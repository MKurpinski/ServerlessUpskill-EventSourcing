namespace Application.Api.Events.Internal
{
    public class CvUploadedEvent : IEvent
    {
        public CvUploadedEvent(string cvUri)
        {
            this.CvUri = cvUri;
        }

        public string CvUri { get; }
    }
}
