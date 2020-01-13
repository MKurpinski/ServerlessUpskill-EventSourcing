using System.Linq;
using System.Threading.Tasks;
using Application.Core.Events;
using Application.EventStore.Facades;
using Application.Search.Dtos;
using Application.Search.Handlers;
using Application.Search.Queries;
using Microsoft.Extensions.Logging;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.EventStore;
using Upskill.Infrastructure.Extensions;

namespace Application.Core.EventHandlers
{
    public class CategoryNameChangedEventHandler : IEventHandler<CategoryNameChangedEvent>
    {
        private readonly IApplicationSearchHandler _applicationSearchHandler;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStoreFacade _eventStore;
        private readonly ILogger<CategoryNameChangedEventHandler> _logger;

        public CategoryNameChangedEventHandler(
            IEventPublisher eventPublisher,
            IApplicationSearchHandler applicationSearchHandler,
            IEventStoreFacade eventStore,
            ILogger<CategoryNameChangedEventHandler> logger)
        {
            _eventPublisher = eventPublisher;
            _applicationSearchHandler = applicationSearchHandler;
            _logger = logger;
            _eventStore = eventStore;
        }

        public async Task Handle(CategoryNameChangedEvent categoryNameChangedEvent)
        {
            var oldCategoryApplications = 
                (await _applicationSearchHandler.GetByCategory(new GetApplicationsByCategoryQuery(categoryNameChangedEvent.OldName)))
                .ToList();

            foreach (var applicationToChange in oldCategoryApplications)
            {
                await HandleSingleApplication(categoryNameChangedEvent, applicationToChange);
            }
        }

        private async Task HandleSingleApplication(
            CategoryNameChangedEvent categoryNameChangedEvent,
            ApplicationDto applicationToChange)
        {
            var applicationCategoryNameChangedEvent =
                new ApplicationCategoryNameChangedEvent(
                    applicationToChange.Id, 
                    categoryNameChangedEvent.NewName,
                    categoryNameChangedEvent.CorrelationId);
            var saveEventResult = await _eventStore.AppendEvent(applicationToChange.Id, applicationCategoryNameChangedEvent);

            if (saveEventResult.Success)
            {
                await _eventPublisher.PublishEvent(applicationCategoryNameChangedEvent);
            }

            _logger.LogErrors(
                $"{nameof(CategoryNameChangedEvent)} for category {categoryNameChangedEvent.NewName} failed for:",
                saveEventResult.Errors);
        }
    }
}