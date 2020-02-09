using System;
using System.Collections.Generic;
using Upskill.Events;
using Upskill.EventStore.Models;
using Upskill.Infrastructure;

namespace Upskill.EventStore.Appliers
{
    public class EventsApplier : IEventsApplier
    {
        private readonly IInvokerProvider _invokerProvider;

        public EventsApplier(IInvokerProvider invokerProvider)
        {
            _invokerProvider = invokerProvider;
        }

        public T ApplyEvents<T>(IReadOnlyCollection<object> events, T root = default) where T : IAggregateRoot
        {
            var aggregate = root == null ? (T)Activator.CreateInstance(typeof(T), true) : root;

            foreach (var @event in events)
            {
               var invoker =
                   _invokerProvider.GetInvoker(aggregate, @event, nameof(IBuildBy<IEvent>.ApplyEvent));
               invoker(aggregate, @event);
            }

            return aggregate;
        }
    }
}