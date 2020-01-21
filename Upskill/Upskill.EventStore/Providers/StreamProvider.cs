using System.Threading.Tasks;
using Streamstone;
using Upskill.EventStore.Models;
using Upskill.EventStore.Tables.Repositories;

namespace Upskill.EventStore.Providers
{
    public class StreamProvider<T> : IStreamProvider<T> where T : IAggregateRoot
    {
        private readonly IStreamLogRepository<T> _streamLogRepository;

        public StreamProvider(IStreamLogRepository<T> streamLogRepository)
        {
            _streamLogRepository = streamLogRepository;
        }

        public async Task<Stream> GetStream(Partition partition)
        {
            var streamOpenResult = await Stream.TryOpenAsync(partition);

            if (streamOpenResult.Found)
            {
                return streamOpenResult.Stream;
            }

            await _streamLogRepository.CreateStreamEntry(partition.Key);
            await Stream.ProvisionAsync(partition);
            var stream = await Stream.OpenAsync(partition);
            return stream;
        }
    }
}