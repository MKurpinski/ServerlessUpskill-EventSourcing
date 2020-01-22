using System.Threading.Tasks;
using Category.Search.Dtos;
using Upskill.Results;

namespace Category.Search.Indexers
{
    public interface ISearchableCategoryIndexer
    {
        Task<IResult> Index(CategoryDto toIndex);
        Task<IResult> Reindex(CategoryDto toIndex);
        Task<IResult> Delete(string id);
    }
}
