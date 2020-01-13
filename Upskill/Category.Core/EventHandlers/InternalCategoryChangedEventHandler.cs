using System.Threading.Tasks;
using Category.Core.Events.External;
using Category.Core.Events.Internal;
using Category.Storage.Tables.Repositories;
using Microsoft.Extensions.Logging;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;

namespace Category.Core.EventHandlers
{
    public class InternalCategoryChangedEventHandler : BaseInternalCategoryChangedEventHandler<InternalCategoryChangedEvent>, IEventHandler<InternalCategoryChangedEvent>
    {
        public InternalCategoryChangedEventHandler(
            ICategoryRepository categoryRepository,
            ILogger<InternalCategoryChangedEvent> logger,
            IEventPublisher eventPublisher) 
            : base(categoryRepository, logger, eventPublisher)
        {
        }

        public async Task Handle(InternalCategoryChangedEvent categoryChangedEvent)
        {
            await this.HandleInternal(categoryChangedEvent);
        }

        protected override async Task<bool> CanBeSaved(InternalCategoryChangedEvent changedEvent)
        {
            var existingCategoryResult = await CategoryRepository.GetById(changedEvent.Id);
            if (!existingCategoryResult.Success)
            {
                return false;
            }

            var existingCategoryWithSameNameResult = await CategoryRepository.GetByName(changedEvent.Name);
            return !existingCategoryWithSameNameResult.Success || (existingCategoryWithSameNameResult.Value.Id == changedEvent.Id);
        }

        protected override async Task HandleSuccessChange(InternalCategoryChangedEvent changedEvent)
        {       await this.EventPublisher.PublishEvent(new CategoryChangedEvent(
                changedEvent.Id,
                changedEvent.Name,
                changedEvent.Description,
                changedEvent.SortOrder,
                changedEvent.CorrelationId));
        }
    }
}
