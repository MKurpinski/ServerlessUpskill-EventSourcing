using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using Upskill.Events;

namespace Upskill.EventsInfrastructure.Dispatchers
{
    public interface IEventDispatcher
    {
        Task<IReadOnlyCollection<IEvent>> Dispatch(params EventGridEvent[] events);
    }
}
