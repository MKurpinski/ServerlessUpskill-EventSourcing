using System.Threading.Tasks;

namespace Category.Search.Managers
{
    public interface ISearchableCategoryReindexManager
    {
        Task StartReindex();
        Task FinishReindexing();
    }
}
