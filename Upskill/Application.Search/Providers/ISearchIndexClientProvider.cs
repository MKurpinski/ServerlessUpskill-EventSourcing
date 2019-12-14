using System.Threading.Tasks;
using Application.Search.Model;
using Microsoft.Azure.Search;

namespace Application.Search.Providers
{
    public interface ISearchIndexClientProvider
    {
        Task<ISearchIndexClient> Get<T>() where T : ISearchable;
    }
}
