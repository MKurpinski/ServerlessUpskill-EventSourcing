using System.Threading.Tasks;
using Upskill.Events;
using Upskill.EventStore.Models;
using Upskill.Results;

namespace Upskill.EventStore
{
    public interface IEventStore
    {
        Task<IMessageResult> AppendEvent<T>(string streamId, IEvent @event) where T: IAggregate;
    }
}
