﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Category.Search.Dtos;
using Category.Search.Indexers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;
using Upskill.EventStore;
using Upskill.EventStore.Appliers;
using Upskill.ReindexGuards;

namespace Category.Api.Functions.Category.RebuildReadModel
{
    public interface ICategoryEntity
    {
        Task QueueEvent(PendingEvent pendingEvent);
        Task ApplyPendingEvents();

        Task Reindex();
        Task Delete();
    }

    public class CategoryEntity : ICategoryEntity
    {
        public Core.Aggregates.Category Category { get; private set; }
        public IList<PendingEvent> PendingEvents { get; private set; }

        [JsonIgnore]
        private readonly ISearchableCategoryIndexer _categoryIndexer;
        [JsonIgnore]
        private readonly IEventStore<Core.Aggregates.Category> _eventStore;
        [JsonIgnore]
        private readonly IEventsApplier _eventsApplier;

        public CategoryEntity(
            ISearchableCategoryIndexer categoryIndexer,
            IEventStore<Core.Aggregates.Category> eventStore,
            IEventsApplier eventsApplier)
        {
            _categoryIndexer = categoryIndexer;
            _eventStore = eventStore;
            _eventsApplier = eventsApplier;

            this.PendingEvents = new List<PendingEvent>();
        }

        public Task QueueEvent(PendingEvent pendingEvent)
        {
            this.PendingEvents.Add(pendingEvent);
            return Task.CompletedTask;
        }

        public async Task ApplyPendingEvents()
        {
            if (!this.PendingEvents.Any())
            {
                return;
            }

            this.Category = _eventsApplier.ApplyEvents(this.PendingEvents.Select(this.PendingEventToEvent).ToList(), this.Category);
            await this.PersistReadModel();
        }


        public async Task Reindex()
        {
            var aggregateResult = await _eventStore.AggregateStream(Entity.Current.EntityKey);
            if (!aggregateResult.Success)
            {
                return;
            }

            this.Category = aggregateResult.Value;
            await this.PersistReadModel();
        }

        private async Task PersistReadModel()
        {
            var toIndex = new CategoryDto(
                this.Category.Id,
                this.Category.Name,
                this.Category.Description,
                this.Category.SortOrder);

            await _categoryIndexer.Reindex(toIndex);
        }

        public object PendingEventToEvent(PendingEvent pendingEvent)
        {
            return JsonConvert.DeserializeObject(pendingEvent.Content, Type.GetType(pendingEvent.EventType));
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
