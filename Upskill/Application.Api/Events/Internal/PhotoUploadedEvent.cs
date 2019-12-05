namespace Application.Api.Events.Internal
{
    public class PhotoUploadedEvent : IEvent
    {
        public PhotoUploadedEvent(string photoUri)
        {
            this.PhotoUri = photoUri;
        }

        public string PhotoUri { get; }
    }
}
