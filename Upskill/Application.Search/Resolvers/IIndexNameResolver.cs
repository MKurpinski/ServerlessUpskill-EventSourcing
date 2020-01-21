using System.Threading.Tasks;
using Application.Search.Enums;
using Application.Search.Models;

namespace Application.Search.Resolvers
{
    public interface IIndexNameResolver
    {
        Task<string> ResolveIndexName<T>(IndexType indexStatus) where T : ISearchable;
    }
}
