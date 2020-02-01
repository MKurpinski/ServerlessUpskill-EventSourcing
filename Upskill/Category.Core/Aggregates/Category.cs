using Category.Core.Events;
using Upskill.Events;
using Upskill.EventStore.Models;

namespace Category.Core.Aggregates
{
    public class Category : IAggregateRoot, IBuildBy<CategoryCreatedEvent>,  IBuildBy<CategoryUpdatedEvent>, IBuildBy<CategoryDeletedEvent>
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int SortOrder { get; private set; }
        public bool IsDeleted { get; private set; }

        public void ApplyEvent(CategoryCreatedEvent categoryCreated)
        {
            this.Id = categoryCreated.Id;
            this.Name = categoryCreated.Name;
            this.Description = categoryCreated.Description;
            this.SortOrder = categoryCreated.SortOrder;
            this.IsDeleted = false;
        }

        public void ApplyEvent(CategoryUpdatedEvent categoryUpdated)
        {
            this.Id = categoryUpdated.Id;
            this.Name = categoryUpdated.Name;
            this.Description = categoryUpdated.Description;
            this.SortOrder = categoryUpdated.SortOrder;
        }

        public void ApplyEvent(CategoryDeletedEvent categoryDeletedEvent)
        {
            this.IsDeleted = true;
        }
    }
}
