using System.Threading.Tasks;
using Application.Storage.Tables.Models;

namespace Application.Storage.Tables.Repositories
{
    public interface ISearchableIndexRepository
    {
        Task Create(SearchableIndex searchableIndex);
        Task UpdateBatch(params SearchableIndex[] indexes);
        Task<SearchableIndex> GetByTypeAndStatus(string type, string status);
    }
}
