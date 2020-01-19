using System.Threading.Tasks;
using Streamstone;
using Upskill.EventStore.Models;

namespace Upskill.EventStore.Providers
{
    public interface IStreamProvider<T> where T: IAggregate
    {
        Task<Stream> GetStream(Partition partition);
    }
}
