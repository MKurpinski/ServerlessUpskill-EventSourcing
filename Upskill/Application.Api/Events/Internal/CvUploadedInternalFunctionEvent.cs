namespace Application.Api.Events.Internal
{
    public class CvUploadedInternalFunctionEvent : IInternalFunctionEvent
    {
        public CvUploadedInternalFunctionEvent(string cvUri)
        {
            this.CvUri = cvUri;
        }

        public string CvUri { get; }
    }
}
