using System.Threading.Tasks;
using Application.Search.Enums;
using Application.Search.Models;
using Upskill.Results;

namespace Application.Search.Managers
{
    public interface IIndexManager
    {
        Task<IDataResult<string>> GetIndexNameByType<T>(IndexType indexType) where T : ISearchable;
        Task StartReindex<T>() where T : ISearchable;
        Task FinishReindexing<T>() where T : ISearchable;
        Task BuildIndex<T>() where T : ISearchable;
    }
}
