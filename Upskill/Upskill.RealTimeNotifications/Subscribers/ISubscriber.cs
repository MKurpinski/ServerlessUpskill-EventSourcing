using System.Threading.Tasks;

namespace Upskill.RealTimeNotifications.Subscribers
{
    public interface ISubscriber
    {
        Task Register(string correlationId, string subscriber);
    }
}
