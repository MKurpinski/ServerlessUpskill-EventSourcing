using System.Threading.Tasks;
using Upskill.EventStore.Models;

namespace Upskill.EventStore.Tables.Repositories
{
    public interface IStreamLogRepository<T> where T: IAggregateRoot
    {
        Task CreateStreamEntry(string id);
    }
}
