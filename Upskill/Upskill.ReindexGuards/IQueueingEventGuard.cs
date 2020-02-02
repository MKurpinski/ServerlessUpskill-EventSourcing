using System.Threading.Tasks;
using Upskill.Events;
using Upskill.EventStore.Models;
using Upskill.Results;
using Upskill.Search.Models;

namespace Upskill.ReindexGuards
{
    public interface IQueueingEventGuard<TSearchable, TAggregate>
        where TSearchable : ISearchable where TAggregate : IAggregateRoot
    {
        Task<IDataResult<string>> ShouldQueueEvent(IEvent @event);
    }
}
