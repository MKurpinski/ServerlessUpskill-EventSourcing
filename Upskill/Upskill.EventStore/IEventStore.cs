using System.Threading.Tasks;
using Upskill.Events;
using Upskill.EventStore.Models;
using Upskill.Results;

namespace Upskill.EventStore
{
    public interface IEventStore<T> where T : IAggregateRoot
    {
        Task<IMessageResult> AppendEvent(string streamId, IEvent @event);
        Task<IDataResult<T>> AggregateStream(string streamId);
    }
}
