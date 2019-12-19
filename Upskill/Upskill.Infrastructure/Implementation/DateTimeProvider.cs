using System;

namespace Upskill.Infrastructure.Implementation
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetCurrentDateTime() => DateTime.UtcNow;
    }
}
