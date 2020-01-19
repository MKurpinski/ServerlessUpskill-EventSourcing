using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Upskill.Cache;
using Upskill.Events;
using Upskill.RealTimeNotifications.Extensions;
using Upskill.RealTimeNotifications.Models;
using Upskill.Results;
using Upskill.Results.Implementation;

namespace Upskill.RealTimeNotifications.Builders
{
    public class NotificationFromEventBuilder : INotificationFromEventBuilder
    {
        private const string FAILURE_EVENT_TYPE_PART = "Failed";
        private const string FINISHED_STATUS = "Finished";
        private readonly ICacheService _cacheService;

        public NotificationFromEventBuilder(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<IDataResult<Notification>> BuildNotification(string eventType, string content)
        {
            var deserializedEvent = JsonConvert.DeserializeObject<StatusEvent>(content);

            if (deserializedEvent?.CorrelationId == null)
            {
                return new FailedDataResult<Notification>();
            }

            var subscriberResult = await _cacheService.Get<string>(deserializedEvent.CorrelationId);

            if (!subscriberResult.Success)
            {
                return new FailedDataResult<Notification>();
            }

            var status = FINISHED_STATUS;
            if (eventType.Contains(FAILURE_EVENT_TYPE_PART, StringComparison.OrdinalIgnoreCase))
            {
                status = deserializedEvent.Status;
            }

            var notification = new Notification(subscriberResult.Value, new NotificationContent(deserializedEvent.CorrelationId, status));
            await _cacheService.Delete(deserializedEvent.CorrelationId);

            return new SuccessfulDataResult<Notification>(notification);
        }

        private class StatusEvent : IBaseStatusEvent
        {
            public string CorrelationId { get; set; }
            public string Status { get; set; }
        }
    }
}