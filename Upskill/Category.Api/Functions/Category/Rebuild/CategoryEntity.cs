using System.Threading.Tasks;
using Category.Search.Dtos;
using Category.Search.Indexers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;
using Upskill.EventStore;

namespace Category.Api.Functions.Category.Rebuild
{
    public interface ICategoryEntity
    {
        Task Rebuild();
        Task Delete();
    }

    public class CategoryEntity : ICategoryEntity
    {
        public Core.Aggregates.Category Category { get; private set; }

        [JsonIgnore]
        private readonly ISearchableCategoryIndexer _categoryIndexer;
        [JsonIgnore]
        private readonly IEventStore<Core.Aggregates.Category> _eventStore;

        public CategoryEntity(
            ISearchableCategoryIndexer categoryIndexer,
            IEventStore<Core.Aggregates.Category> eventStore)
        {
            _categoryIndexer = categoryIndexer;
            _eventStore = eventStore;
        }

        public async Task Rebuild()
        {
            var aggregateResult = await _eventStore.AggregateStream(Entity.Current.EntityKey);
            if (!aggregateResult.Success)
            {
                return;
            }

            this.Category = aggregateResult.Value;
            var toIndex = new CategoryDto(
                this.Category.Id,
                this.Category.Name, 
                this.Category.Description,
                this.Category.SortOrder);

            await _categoryIndexer.Reindex(toIndex);
        }

        public Task Delete()
        {
            Entity.Current.DeleteState();
            return Task.CompletedTask;
        }

        [FunctionName(nameof(CategoryEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
            => ctx.DispatchAsync<CategoryEntity>();
    }
}
