using System;
using Upskill.Results;

namespace Upskill.EventsInfrastructure.Providers
{
    public interface IEventTypeProvider
    {
        IDataResult<Type> ResolveEventType(string type);
    }
}
