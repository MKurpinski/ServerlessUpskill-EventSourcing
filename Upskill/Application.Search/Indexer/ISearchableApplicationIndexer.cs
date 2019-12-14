using System.Threading.Tasks;
using Application.Search.Dtos;

namespace Application.Search.Indexer
{
    public interface ISearchableApplicationIndexer
    {
        Task Index(ApplicationDto toIndex);
    }
}
