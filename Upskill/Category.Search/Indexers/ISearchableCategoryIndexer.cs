using System.Threading.Tasks;
using Category.Search.Dtos;
using Upskill.Results;

namespace Category.Search.Indexers
{
    public interface ISearchableCategoryIndexer
    {
        Task Index(CategoryDto toIndex);
        Task Reindex(CategoryDto toIndex);
        Task<IResult> Delete(string id);
    }
}
