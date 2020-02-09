using System.Threading.Tasks;
using Upskill.Search.Tables.Models;

namespace Upskill.Search.Tables.Repositories
{
    public interface ISearchableIndexRepository
    {
        Task Create(SearchableIndex searchableIndex);
        Task UpdateBatch(params SearchableIndex[] indexes);
        Task<SearchableIndex> GetByTypeAndStatus(string type, string status);
    }
}
