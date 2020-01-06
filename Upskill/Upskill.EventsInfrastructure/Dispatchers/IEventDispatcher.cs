using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;

namespace Upskill.EventsInfrastructure.Dispatchers
{
    public interface IEventDispatcher
    {
        Task Dispatch(params EventGridEvent[] events);
    }
}
