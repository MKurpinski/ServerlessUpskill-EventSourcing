using System.Threading.Tasks;
using Application.PushNotifications.Providers;
using Microsoft.Azure.NotificationHubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Application.PushNotifications.Senders
{
    public abstract class Sender : ISender
    {
        protected readonly NotificationHubClient HubClient;

        protected Sender(IHubConnectionProvider provider)
        {
            HubClient = provider.Get();
        }

        public abstract Task Send<T>(T payload, string message, string[] tags = null);

        protected string SerializePayload<T>(T payload)
        {
            return JsonConvert.SerializeObject(payload, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}
