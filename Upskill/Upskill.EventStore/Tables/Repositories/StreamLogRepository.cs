using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upskill.EventStore.Models;
using Upskill.EventStore.Tables.Models;
using Upskill.Storage.Table.Providers;
using Upskill.Storage.Table.Repositories;

namespace Upskill.EventStore.Tables.Repositories
{
    public class StreamLogRepository<T> : Repository<StreamLog>, IStreamLogRepository<T> where T : IAggregateRoot
    {
        private const string STREAM_LOG_TABLE_SUFFIX = "StreamLog";
        public StreamLogRepository(ITableClientProvider tableClientProvider) 
            : base(tableClientProvider, $"{typeof(T).Name}{STREAM_LOG_TABLE_SUFFIX}")
        {
        }

        public async Task CreateStreamEntry(string id)
        {
            await this.CreateOrUpdate(new StreamLog(id));
        }

        public async Task<IReadOnlyCollection<string>> GetStreams()
        {
            var all = await this.GetAll();
            return all.Select(x => x.Id).ToList();
        }
    }
}