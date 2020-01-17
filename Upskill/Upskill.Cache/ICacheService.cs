using System.Threading.Tasks;
using Upskill.Results;

namespace Upskill.Cache
{
    public interface ICacheService
    {
        Task<IDataResult<T>> Get<T>(string key);
        Task Set<T>(string key, T value);
        Task Delete(string key);
    }
}