using System.Threading.Tasks;

namespace Application.PushNotifications.Senders
{
    public interface ISender
    {
        Task Send<T>(T payload, string message, string[] tags = null);
    }
}
