using System.Threading.Tasks;
using Application.Search.Dtos;

namespace Application.Search.Indexers
{
    public interface ISearchableApplicationIndexer
    {
        Task Index(ApplicationDto toIndex);
        Task Reindex(ApplicationDto toIndex);
        Task BuildNewIndex();
    }
}
