using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Nito.AsyncEx;
using Streamstone;
using Upskill.Events;
using Upskill.Events.Extensions;
using Upskill.EventStore.Builder;
using Upskill.EventStore.Models;
using Upskill.EventStore.Providers;
using Upskill.Results;
using Upskill.Results.Implementation;
using Upskill.Storage.Table.Providers;

namespace Upskill.EventStore
{
    public class EventStore<T> : IEventStore<T> where T : IAggregateRoot
    {
        private const string STREAMS_TABLE_SUFFIX = "Stream";
        private const string APPLY_EVENT = "ApplyEvent";
        private readonly AsyncLazy<CloudTable> _lazyTableClient;
        private readonly IEventDataBuilder _eventDataBuilder;
        private readonly IStreamProvider<T> _streamProvider;

        public EventStore(
            ITableClientProvider tableClientProvider,
            IEventDataBuilder eventDataBuilder,
            IStreamProvider<T> streamProvider)
        {
            _eventDataBuilder = eventDataBuilder;
            _streamProvider = streamProvider;
            _lazyTableClient =
                new AsyncLazy<CloudTable>(() => tableClientProvider.Get($"{typeof(T).Name}{STREAMS_TABLE_SUFFIX}"));
        }

        public async Task<IMessageResult> AppendEvent(string streamId, IEvent @event)
        {
            var tableClient = await _lazyTableClient;
            var partition = new Partition(tableClient, streamId);

            var stream = await _streamProvider.GetStream(partition);

            try
            {
                var preparedEvent = _eventDataBuilder.BuildEventData<T>(@event);
                await Stream.WriteAsync(stream, preparedEvent);
                return new SuccessfulMessageResult();
            }
            catch (ConcurrencyConflictException ex)
            {
                return new FailedMessageResult(nameof(ConcurrencyConflictException), ex.Message);
            }
            catch (Exception ex)
            {
                return new FailedMessageResult(nameof(Exception), ex.Message);
            }
        }

        public async Task<IDataResult<T>> AggregateStream(string streamId)
        {
            var aggregate = (T) Activator.CreateInstance(typeof(T), true);

            var events = await this.GetEvents(streamId);

            if (!events.Any())
            {
                return new FailedDataResult<T>();
            }

            foreach (var @event in events)
            {
                aggregate.Invoke(APPLY_EVENT, @event);
            }

            return new SuccessfulDataResult<T>(aggregate);
        }

        private async Task<IReadOnlyCollection<object>> GetEvents(string streamId)
        {
            const int sliceSize = 1000;
            var allEvents = new List<object>();
            StreamSlice<EventProperties> slice;
            var startVersion = 1;
            var partition = new Partition(await _lazyTableClient, streamId);

            do
            {
                slice = await Stream.ReadAsync(partition, startVersion, sliceSize);

                allEvents.AddRange(slice.Events.Select(this.ToEvent));

                startVersion += sliceSize;
            }
            while (!slice.IsEndOfStream);

            return allEvents;
        }

        private object ToEvent(PropertyMap eventProperties)
        {
            var type = eventProperties[nameof(EventStorageData.EventType)].StringValue;
            var typeOfEvent = Type.GetType(type);
            var data = eventProperties[nameof(EventStorageData.Content)].StringValue;

            return JsonConvert.DeserializeObject(data, typeOfEvent);
        }
    }
}