using System.Threading.Tasks;
using Upskill.Search.Enums;
using Upskill.Search.Models;

namespace Upskill.Search.Resolvers
{
    public interface IIndexNameResolver
    {
        Task<string> ResolveIndexName<T>(IndexType indexStatus) where T : ISearchable;
    }
}
