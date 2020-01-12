using System.Threading.Tasks;
using Upskill.Events;
using Upskill.Results;

namespace Category.EventStore.Facades
{
    public interface IEventStoreFacade
    {
        Task<IMessageResult> AppendEvent(string streamId, IEvent @event);
    }
}
