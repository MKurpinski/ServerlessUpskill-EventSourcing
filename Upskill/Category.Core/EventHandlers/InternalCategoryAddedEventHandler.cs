using System.Threading.Tasks;
using Category.Core.Events.External;
using Category.Core.Events.Internal;
using Category.Storage.Tables.Repositories;
using Microsoft.Extensions.Logging;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;

namespace Category.Core.EventHandlers
{
    public class InternalCategoryAddedEventHandler : BaseInternalCategoryChangedEventHandler<InternalCategoryAddedEvent>, IEventHandler<InternalCategoryAddedEvent>
    {
        public InternalCategoryAddedEventHandler(
            ICategoryRepository categoryRepository,
            ILogger<InternalCategoryAddedEvent> logger,
            IEventPublisher eventPublisher)
            : base(categoryRepository, logger, eventPublisher)
        {
        }

        public async Task Handle(InternalCategoryAddedEvent categoryChangedEvent)
        {
            await this.HandleInternal(categoryChangedEvent);
        }

        protected override async Task<bool> CanBeSaved(InternalCategoryAddedEvent changedEvent)
        {
            var existingCategoryResult = await CategoryRepository.GetByName(changedEvent.Name);
            return !existingCategoryResult.Success;
        }

        protected override async Task HandleSuccessChange(InternalCategoryAddedEvent changedEvent)
        {
            await this.EventPublisher.PublishEvent(new CategoryAddedEvent(
                changedEvent.Id,
                changedEvent.Name,
                changedEvent.Description,
                changedEvent.SortOrder,
                changedEvent.CorrelationId));
        }
    }
}
