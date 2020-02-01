using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Search.Dtos;
using Application.Search.Indexers;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;
using Upskill.Events;
using Upskill.EventStore;
using Upskill.EventStore.Appliers;

namespace Application.Api.Functions.RebuildReadModel
{
    public interface IApplicationEntity
    {
        Task Reindex();
        Task Delete();
        Task QueueEvent(IEvent @event);
        Task ApplyPendingEvents();
    }

    public class ApplicationEntity : IApplicationEntity
    {
        public Core.Aggregates.Application Application { get; private set; }
        public IList<IEvent> PendingEvents { get; private set; }

        [JsonIgnore]
        private readonly ISearchableApplicationIndexer _applicationIndexer;
        [JsonIgnore]
        private readonly IEventStore<Core.Aggregates.Application> _eventStore;
        [JsonIgnore]
        private readonly IMapper _mapper;
        [JsonIgnore]
        private readonly IEventsApplier _eventsApplier;

        public ApplicationEntity(
            ISearchableApplicationIndexer applicationIndexer,
            IEventStore<Core.Aggregates.Application> eventStore,
            IMapper mapper,
            IEventsApplier eventsApplier)
        {
            _applicationIndexer = applicationIndexer;
            _eventStore = eventStore;
            _mapper = mapper;
            _eventsApplier = eventsApplier;
            this.PendingEvents = new List<IEvent>();
        }

        public Task QueueEvent(IEvent @event)
        {
            this.PendingEvents.Add(@event);
            return Task.CompletedTask;
        }

        public async Task ApplyPendingEvents()
        {
            if (!this.PendingEvents.Any())
            {
                return;
            }

            this.Application = _eventsApplier.ApplyEvents(this.PendingEvents.ToList(), this.Application);
            await this.PersistReadModel();
        }

        public async Task Reindex()
        {
            var aggregateResult = await _eventStore.AggregateStream(Entity.Current.EntityKey);
            if (!aggregateResult.Success)
            {
                return;
            }

            this.Application = aggregateResult.Value;
            await this.PersistReadModel();
        }

        public Task Delete()
        {
            Entity.Current.DeleteState();
            return Task.CompletedTask;
        }

        private async Task PersistReadModel()
        {
            var mapped = _mapper.Map<Core.Aggregates.Application, ApplicationDto>(this.Application);
            await _applicationIndexer.Reindex(mapped);
        }

        [FunctionName(nameof(ApplicationEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
            => ctx.DispatchAsync<ApplicationEntity>();
    }
}
