using Category.Core.Events.External;
using Upskill.EventStore.Models;

namespace Category.Core.Aggregates
{
    public class Category : IAggregateRoot
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int SortOrder { get; private set; }
        public bool IsDeleted { get; private set; }

        private void ApplyEvent(CategoryCreatedEvent categoryCreated)
        {
            this.Id = categoryCreated.Id;
            this.Name = categoryCreated.Name;
            this.Description = categoryCreated.Description;
            this.SortOrder = categoryCreated.SortOrder;
            this.IsDeleted = false;
        }

        private void ApplyEvent(CategoryUpdatedEvent categoryUpdated)
        {
            this.Id = categoryUpdated.Id;
            this.Name = categoryUpdated.Name;
            this.Description = categoryUpdated.Description;
            this.SortOrder = categoryUpdated.SortOrder;
        }

        private void ApplyEvent(CategoryDeletedEvent categoryDeletedEvent)
        {
            this.IsDeleted = true;
        }
    }
}
