using System.Threading.Tasks;
using Category.Core.Events;
using Upskill.Events;

namespace Category.Core.EventHandlers
{
    public class CategoryChangedEventHandler: IEventHandler<CategoryChangedEvent>
    {
        public Task Handle(CategoryChangedEvent categoryChangedEvent)
        {
            return Task.CompletedTask;
        }
    }
}
