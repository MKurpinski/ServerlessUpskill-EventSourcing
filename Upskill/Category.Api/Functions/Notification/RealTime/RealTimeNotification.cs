using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Upskill.RealTimeNotifications.Builders;
using Upskill.RealTimeNotifications.Constants;
using Upskill.Telemetry.CorrelationInitializers;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Category.Api.Functions.Notification.RealTime
{
    public class RealTimeNotification
    {
        private readonly INotificationFromEventBuilder _notificationFromEventBuilder;
        private readonly ICorrelationInitializer _correlationInitializer;

        public RealTimeNotification(
            INotificationFromEventBuilder notificationFromEventBuilder, 
            ICorrelationInitializer correlationInitializer)
        {
            _notificationFromEventBuilder = notificationFromEventBuilder;
            _correlationInitializer = correlationInitializer;
        }

        private const string NOTIFICATION_HUB_NAME = "CategoryNotifications";
        private const string USER_ID_HEADER = NotificationUserId.HeaderSearchPattern;

        [FunctionName("negotiate")]
        public IActionResult GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Post)] HttpRequest req,
            [SignalRConnectionInfo(
                HubName = NOTIFICATION_HUB_NAME,
                UserId = USER_ID_HEADER)] SignalRConnectionInfo connectionInfo)
        {
            return new OkObjectResult(connectionInfo);
        }

        [FunctionName(nameof(EventNotificationHandler))]
        public async Task EventNotificationHandler(
            [EventGridTrigger] EventGridEvent @event,
            [SignalR(HubName = NOTIFICATION_HUB_NAME)] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            _correlationInitializer.Initialize(@event.Subject);
            var messageToPushResult =
                await _notificationFromEventBuilder.BuildNotification(@event.EventType, @event.Data as string);

            if (!messageToPushResult.Success)
            {
                return;
            }

            var signalRMessage = new SignalRMessage
            {
                Target = nameof(messageToPushResult),
                UserId = messageToPushResult.Value.To,
                Arguments = new object[]{ messageToPushResult.Value.Content },
            };

            await signalRMessages.AddAsync(signalRMessage);
        }
    }
}
