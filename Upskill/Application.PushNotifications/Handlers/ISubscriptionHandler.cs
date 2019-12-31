using System.Threading.Tasks;
using Application.PushNotifications.Commands;
using Application.PushNotifications.Dtos;
using Upskill.Results;

namespace Application.PushNotifications.Handlers
{
    public interface ISubscriptionHandler
    {
        Task<IDataResult<RegistrationIdDto>> CreateSubscription(CreateSubscriptionCommand command);
        Task<IResult> DeleteSubscription(DeleteSubscriptionCommand command);
    }
}
