using System.Threading.Tasks;
using Upskill.Events;
using Upskill.EventStore.Models;
using Upskill.Results;

namespace Upskill.EventStore
{
    public interface IEventStore<T> where T : IAggregate
    {
        Task<IMessageResult> AppendEvent(string streamId, IEvent @event);
    }
}
