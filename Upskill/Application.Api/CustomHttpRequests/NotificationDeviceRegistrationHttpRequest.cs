namespace Application.Api.CustomHttpRequests
{
    public class NotificationDeviceRegistrationHttpRequest
    {
        public string Platform { get; set; }
        public string Handle { get; set; }
        public string[] Tags { get; set; }
    }
}
