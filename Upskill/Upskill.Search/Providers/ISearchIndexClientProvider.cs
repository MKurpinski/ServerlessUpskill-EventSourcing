using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Upskill.Search.Enums;
using Upskill.Search.Models;

namespace Upskill.Search.Providers
{
    public interface ISearchIndexClientProvider
    {
        Task<ISearchIndexClient> Get<T>(IndexType indexType = IndexType.Active) where T : ISearchable;
    }
}
