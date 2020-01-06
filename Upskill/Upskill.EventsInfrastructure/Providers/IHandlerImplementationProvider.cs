using System;

namespace Upskill.EventsInfrastructure.Providers
{
    public interface IHandlerImplementationProvider
    {
        bool TryResolveHandlers(Type type, out object[] handlers);
    }
}
