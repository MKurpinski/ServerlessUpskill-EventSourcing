using System.Threading.Tasks;
using Application.Search.Models;
using Microsoft.Azure.Search;

namespace Application.Search.Providers
{
    public interface ISearchIndexClientProvider
    {
        Task<ISearchIndexClient> Get<T>() where T : ISearchable;
    }
}
