using System;

namespace Upskill.Infrastructure
{
    public interface IDateTimeProvider
    {
        DateTime GetCurrentDateTime();
    }
}
