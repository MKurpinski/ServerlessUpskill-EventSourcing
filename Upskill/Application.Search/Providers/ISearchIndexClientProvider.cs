using System.Threading.Tasks;
using Application.Search.Enums;
using Application.Search.Models;
using Microsoft.Azure.Search;

namespace Application.Search.Providers
{
    public interface ISearchIndexClientProvider
    {
        Task<ISearchIndexClient> Get<T>(IndexType indexType = IndexType.Active) where T : ISearchable;
    }
}
