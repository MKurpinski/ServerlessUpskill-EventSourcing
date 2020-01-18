using System.Threading.Tasks;

namespace Upskill.Events
{
    public interface IEventHandler<T> where T: IEvent
    {
        Task Handle(T @event);
    }
}