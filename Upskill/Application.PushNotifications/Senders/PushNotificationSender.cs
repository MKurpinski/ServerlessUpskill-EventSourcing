using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.PushNotifications.Senders
{
    public class PushNotificationSender : IPushNotificationSender
    {
        private readonly IEnumerable<ISender> _platformSpecificSenders;

        public PushNotificationSender(IEnumerable<ISender> platformSpecificSenders)
        {
            _platformSpecificSenders = platformSpecificSenders;
        }

        public async Task SendNotification<T>(T notification, string message, string[] tags = null)
        {
            var sendTasks = _platformSpecificSenders.Select(x => x.Send(notification, message, tags));
            await Task.WhenAll(sendTasks);
        }
    }
}