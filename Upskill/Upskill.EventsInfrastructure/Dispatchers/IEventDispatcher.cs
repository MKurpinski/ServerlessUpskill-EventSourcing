using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using Upskill.EventsInfrastructure.Dtos;

namespace Upskill.EventsInfrastructure.Dispatchers
{
    public interface IEventDispatcher
    {
        Task<IReadOnlyCollection<DispatchedEvent>> Dispatch(params EventGridEvent[] events);
    }
}
