using System.Threading.Tasks;
using Upskill.Events;
using Upskill.Results;

namespace Upskill.EventStore
{
    public interface IEventStore
    {
        Task<IMessageResult> AppendEvent(string streamId, IEvent @event);
    }
}
