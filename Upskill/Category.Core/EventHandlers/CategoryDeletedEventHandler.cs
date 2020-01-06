using System.Threading.Tasks;
using Category.Core.Events;
using Upskill.Events;

namespace Category.Core.EventHandlers
{
    public class CategoryDeletedEventHandler : IEventHandler<CategoryDeletedEvent>
    {
        public Task Handle(CategoryDeletedEvent categoryDeletedEvent)
        {
            return Task.CompletedTask;
        }
    }
}
