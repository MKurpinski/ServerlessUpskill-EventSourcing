using System.Threading.Tasks;
using Application.Storage.Table.Model;

namespace Application.Storage.Table.Repository
{
    public interface ISearchableIndexRepository
    {
        Task Create(SearchableIndex searchableIndex);
        Task<SearchableIndex> GetByTypeAndStatus(string type, string status);
    }
}
