using System.Threading.Tasks;
using Upskill.Events;
using Upskill.Results;

namespace Application.EventStore.Facades
{
    public interface IEventStoreFacade
    {
        Task<IMessageResult> AppendEvent(string streamId, IEvent @event);
    }
}
