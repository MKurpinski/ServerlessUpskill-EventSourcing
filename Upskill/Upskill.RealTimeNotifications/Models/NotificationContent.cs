namespace Upskill.RealTimeNotifications.Models
{
    public class NotificationContent
    {
        public string CorrelationId { get; }
        public string Status { get; }

        public NotificationContent(string correlationId, string status)
        {
            CorrelationId = correlationId;
            Status = status;
        }
    }
}