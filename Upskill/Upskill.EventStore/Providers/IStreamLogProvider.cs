using System.Collections.Generic;
using System.Threading.Tasks;
using Upskill.EventStore.Models;

namespace Upskill.EventStore.Providers
{
    public interface IStreamLogProvider<T> where T:IAggregateRoot
    {
        Task<IReadOnlyCollection<string>> GetStreams();
    }
}
