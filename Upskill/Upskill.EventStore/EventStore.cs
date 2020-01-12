using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Nito.AsyncEx;
using Streamstone;
using Upskill.Events;
using Upskill.EventStore.Builder;
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

        public async Task<IMessageResult> AppendEvent(string streamId, IEvent @event)
        {
            var tableClient = await _lazyTableClient;
            var partition = new Partition(tableClient, streamId);

            var stream = await Stream.OpenAsync(partition);

            try
            {
                var preparedEvent = _eventDataBuilder.BuildEventData(@event);
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
    }
}