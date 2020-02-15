using System;
using Upskill.Infrastructure.Enums;

namespace Upskill.LogChecker.Dtos
{
    public class LogDto
    {
        public string CorrelationId { get; }
        public OperationStatus Status { get; }
        public string Description { get; }
        public DateTime Timestamp { get; }

        public LogDto(
            string correlationId,
            OperationStatus status,
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
