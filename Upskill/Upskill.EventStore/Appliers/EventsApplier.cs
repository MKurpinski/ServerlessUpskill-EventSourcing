using System;
using System.Collections.Generic;
using Upskill.Events;
using Upskill.Events.Extensions;
using Upskill.EventStore.Models;

namespace Upskill.EventStore.Appliers
{
    public class EventsApplier : IEventsApplier
    {
        public T ApplyEvents<T>(IReadOnlyCollection<object> events, T root = default) where T : IAggregateRoot
        {
            var aggregate = root == null ? (T)Activator.CreateInstance(typeof(T), true) : root;

            foreach (var @event in events)
            {
                aggregate.Invoke(nameof(IBuildBy<IEvent>.ApplyEvent), @event);
            }

            return aggregate;
        }
    }
}