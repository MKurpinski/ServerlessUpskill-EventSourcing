using System.Threading.Tasks;

namespace Application.Search.Managers
{
    public interface ISearchableApplicationReindexManager
    {
        Task StartReindex();
        Task FinishReindexing();
    }
}
