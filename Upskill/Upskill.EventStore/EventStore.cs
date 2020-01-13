﻿using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Nito.AsyncEx;
using Streamstone;
using Upskill.Events;
using Upskill.EventStore.Builder;
using Upskill.EventStore.Models;
using Upskill.Results;
using Upskill.Results.Implementation;
using Upskill.Storage.Table.Providers;

namespace Upskill.EventStore
{
    public class EventStore : IEventStore
    {
        private const string STREAMS_TABLE_NAME = "Stream";
        private readonly AsyncLazy<CloudTable> _lazyTableClient;
        private readonly IEventDataBuilder _eventDataBuilder;

        public EventStore(
            ITableClientProvider tableClientProvider,
            IEventDataBuilder eventDataBuilder)
        {
            _eventDataBuilder = eventDataBuilder;
            _lazyTableClient = new AsyncLazy<CloudTable>(() => tableClientProvider.Get(STREAMS_TABLE_NAME));
        }

        public async Task<IMessageResult> AppendEvent<T>(string streamId, IEvent @event) where T : IAggregate
        {
            var tableClient = await _lazyTableClient;
            var partition = new Partition(tableClient, streamId);


            var stream = await this.GetStream(partition);

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

        private async Task<Stream> GetStream(Partition partition)
        {
            var streamOpenResult = await Stream.TryOpenAsync(partition);

            if (streamOpenResult.Found)
            {
                return streamOpenResult.Stream;
            }

            await Stream.ProvisionAsync(partition);
            var stream = await Stream.OpenAsync(partition);
            return stream;
        }
    }
}