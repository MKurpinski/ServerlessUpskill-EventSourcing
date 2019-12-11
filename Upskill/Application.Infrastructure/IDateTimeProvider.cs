using System;

namespace Application.Infrastructure
{
    public interface IDateTimeProvider
    {
        DateTime GetCurrentDateTime();
    }
}
