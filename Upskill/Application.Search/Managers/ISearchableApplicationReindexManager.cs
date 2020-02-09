using System.Threading.Tasks;
using Upskill.Results;

namespace Application.Search.Managers
{
    public interface ISearchableApplicationReindexManager
    {
        Task StartReindex();
        Task FinishReindexing();
    }
}
