using System.Linq;
using System.Threading.Tasks;
using Application.PushNotifications.Providers;

namespace Application.PushNotifications.Senders
{
    class ApnsSender : Sender
    {
        public ApnsSender(IHubConnectionProvider provider) : base(provider)
        {
        }

        public override async Task Send<T>(T payload, string message, string[] tags = null)
        {
            var payloadToSend = this.SerializePayload(new Payload(new Aps(message, payload)));

            if (tags?.Any() == true)
            {
                await HubClient.SendAppleNativeNotificationAsync(payloadToSend, tags);
                return;
            }

            await HubClient.SendAppleNativeNotificationAsync(payloadToSend);
        }

        public class Payload
        {
            public Payload(Aps aps)
            {
                Aps = aps;
            }

            public Aps Aps { get; }
        }

        public class Aps
        {
            public Aps(string alert, object payload)
            {
                Alert = alert;
                Payload = payload;
            }

            public string Alert { get; }
            public object Payload { get; }
        }
    }
}
