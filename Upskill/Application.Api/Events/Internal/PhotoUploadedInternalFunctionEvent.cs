namespace Application.Api.Events.Internal
{
    public class PhotoUploadedInternalFunctionEvent : IInternalFunctionEvent
    {
        public PhotoUploadedInternalFunctionEvent(string photoUri)
        {
            this.PhotoUri = photoUri;
        }

        public string PhotoUri { get; }
    }
}
