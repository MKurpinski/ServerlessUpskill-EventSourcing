namespace Application.PushNotifications.Commands
{
    public class CreateSubscriptionCommand
    {
        public string Platform { get; }
        public string Handle { get; }
        public string[] Tags { get; }

        public CreateSubscriptionCommand(string platform, string handle, string[] tags)
        {
            Platform = platform;
            Handle = handle;
            Tags = tags;
        }
    }
}
