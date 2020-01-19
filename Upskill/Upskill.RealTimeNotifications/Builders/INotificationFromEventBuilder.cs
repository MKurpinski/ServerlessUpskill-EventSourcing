using System.Threading.Tasks;
using Upskill.RealTimeNotifications.Models;
using Upskill.Results;

namespace Upskill.RealTimeNotifications.Builders
{
    public interface INotificationFromEventBuilder
    {
        Task<IDataResult<Notification>> BuildNotification(string eventType, string content);
    }
}
