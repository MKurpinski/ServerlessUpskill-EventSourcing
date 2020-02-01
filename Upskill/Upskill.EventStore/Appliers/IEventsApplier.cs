using System.Collections.Generic;
using Upskill.EventStore.Models;

namespace Upskill.EventStore.Appliers
{
    public interface IEventsApplier
    {
        T ApplyEvents<T>(IReadOnlyCollection<object> events, T root = default) where T : IAggregateRoot;
    }
}
