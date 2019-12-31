namespace Application.PushNotifications.Dtos
{
    public class RegistrationIdDto
    {
        public RegistrationIdDto(string registrationId)
        {
            RegistrationId = registrationId;
        }

        public string RegistrationId { get; }
    }
}
