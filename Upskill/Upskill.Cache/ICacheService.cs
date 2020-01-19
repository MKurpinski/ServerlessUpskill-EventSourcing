using System;
using System.Threading.Tasks;
using Upskill.Results;

namespace Upskill.Cache
{
    public interface ICacheService
    {
        Task<IDataResult<T>> Get<T>(string key);
        Task Set<T>(string key, T value, TimeSpan? expiresOn = null);
        Task Delete(string key);
    }
}