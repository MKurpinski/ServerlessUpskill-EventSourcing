namespace Application.PushNotifications.Commands
{
    public class DeleteSubscriptionCommand
    {
        public DeleteSubscriptionCommand(string registrationId)
        {
            RegistrationId = registrationId;
        }

        public string RegistrationId { get; }
    }
}