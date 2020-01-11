using System.Linq;
using System.Threading.Tasks;
using Application.Core.Events;
using Application.Search.Handlers;
using Application.Search.Queries;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;

namespace Application.Core.EventHandlers
{
    public class CategoryNameChangedEventHandler : IEventHandler<CategoryNameChangedEvent>
    {
        private readonly IApplicationSearchHandler _applicationSearchHandler;
        private readonly IEventPublisher _eventPublisher;

        public CategoryNameChangedEventHandler(
            IEventPublisher eventPublisher,
            IApplicationSearchHandler applicationSearchHandler)
        {
            _eventPublisher = eventPublisher;
            _applicationSearchHandler = applicationSearchHandler;
        }

        public async Task Handle(CategoryNameChangedEvent categoryNameChangedEvent)
        {
            var oldCategoryApplications = 
                (await _applicationSearchHandler.GetByCategory(new GetApplicationsByCategoryQuery(categoryNameChangedEvent.OldName)))
                .ToList();

            var tasks = oldCategoryApplications.Select(x =>
            {
                var createdEvent = new ApplicationCategoryNameChangedEvent(x.Id, categoryNameChangedEvent.NewName);
                // save event
                return _eventPublisher.PublishEvent(createdEvent);
            });

            await Task.WhenAll(tasks);
        }
    }
}