using System.Linq;
using System.Threading.Tasks;
using Application.PushNotifications.Providers;

namespace Application.PushNotifications.Senders
{
    public class FcmSender : Sender
    {
        public FcmSender(IHubConnectionProvider provider) : base(provider)
        {
        }

        public override async Task Send<T>(T payload, string message, string[] tags = null)
        {
            var payloadToSend = this.SerializePayload(new Payload(new Data(message, payload)));

            if (tags?.Any() == true)
            {
                await HubClient.SendFcmNativeNotificationAsync(payloadToSend, tags);
                return;
            }

            await HubClient.SendFcmNativeNotificationAsync(payloadToSend);
        }

        public class Payload
        {
            public Payload(Data data)
            {
                Data = data;
            }

            public Data Data { get; }
        }

        public class Data
        {
            public Data(
                string message,
                object payload)
            {
                Message = message;
                Payload = payload;
            }

            public string Message { get; }
            public object Payload { get; }
        }
    }
}
