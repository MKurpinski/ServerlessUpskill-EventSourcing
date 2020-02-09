using System.Collections.Generic;
using System.Threading.Tasks;
using Upskill.EventStore.Models;
using Upskill.EventStore.Tables.Repositories;

namespace Upskill.EventStore.Providers
{
    public class StreamLogProvider<T> : IStreamLogProvider<T> where T : IAggregateRoot
    {
        private readonly IStreamLogRepository<T> _streamLogRepository;

        public StreamLogProvider(IStreamLogRepository<T> streamLogRepository)
        {
            _streamLogRepository = streamLogRepository;
        }

        public Task<IReadOnlyCollection<string>> GetStreams()
        {
            return _streamLogRepository.GetStreams();
        }
    }
}