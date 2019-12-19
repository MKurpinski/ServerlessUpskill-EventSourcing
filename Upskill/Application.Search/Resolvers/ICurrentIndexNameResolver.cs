using System.Threading.Tasks;

namespace Application.Search.Resolvers
{
    public interface ICurrentIndexNameResolver
    {
        Task<string> ResolveCurrentIndexName<T>();
    }
}
