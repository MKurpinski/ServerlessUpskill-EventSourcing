using System;

namespace Upskill.LogChecker.Dtos
{
    public class LogDto
    {
        public string CorrelationId { get; }
        public string Status { get; }
        public string Description { get; }
        public DateTime Timestamp { get; }

        public LogDto(
            string correlationId,
            string status,
            string description,
            DateTime timestamp)
        {
            CorrelationId = correlationId;
            Status = status;
            Description = description;
            Timestamp = timestamp;
        }
    }
}
