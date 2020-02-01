using System.Threading.Tasks;
using Application.Search.Dtos;
using Application.Search.Indexers;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;
using Upskill.EventStore;

namespace Application.Api.Functions.Rebuild
{
    public interface IApplicationEntity
    {
        Task Rebuild();
        Task Delete();
    }

    public class ApplicationEntity : IApplicationEntity
    {
        public Core.Aggregates.Application Application { get; private set; }

        [JsonIgnore]
        private readonly ISearchableApplicationIndexer _applicationIndexer;
        [JsonIgnore]
        private readonly IEventStore<Core.Aggregates.Application> _eventStore;
        [JsonIgnore]
        private readonly IMapper _mapper;

        public ApplicationEntity(
            ISearchableApplicationIndexer applicationIndexer,
            IEventStore<Core.Aggregates.Application> eventStore,
            IMapper mapper)
        {
            _applicationIndexer = applicationIndexer;
            _eventStore = eventStore;
            _mapper = mapper;
        }

        public async Task Rebuild()
        {
            var aggregateResult = await _eventStore.AggregateStream(Entity.Current.EntityKey);
            if (!aggregateResult.Success)
            {
                return;
            }

            this.Application = aggregateResult.Value;
            var mapped = _mapper.Map<Core.Aggregates.Application, ApplicationDto>(this.Application);
            await _applicationIndexer.Reindex(mapped);
        }

        public Task Delete()
        {
            Entity.Current.DeleteState();
            return Task.CompletedTask;
        }

        [FunctionName(nameof(ApplicationEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
            => ctx.DispatchAsync<ApplicationEntity>();
    }
}
