using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Upskill.EventsInfrastructure.Providers
{
    public class HandlerImplementationProvider : IHandlerImplementationProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public HandlerImplementationProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public bool TryResolveHandlers(Type type, out object[] handlers)
        {
            handlers = default;
            try
            {
                handlers = _serviceProvider.GetServices(type).ToArray();
                return handlers.Any();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}