using Microsoft.Azure.NotificationHubs;
using Upskill.Results;

namespace Application.PushNotifications.Factories
{
    public interface IRegistrationDescriptionFactory
    {
        IDataResult<RegistrationDescription> Build(
            string registrationId,
            string platform,
            string handle,
            string[] tags);
    }
}
