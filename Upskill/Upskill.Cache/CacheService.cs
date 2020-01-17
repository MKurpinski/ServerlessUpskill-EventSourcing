using System.Threading.Tasks;
using Nito.AsyncEx;
using StackExchange.Redis;
using Upskill.Cache.Providers;
using Upskill.Cache.Utils;
using Upskill.Results;
using Upskill.Results.Implementation;

namespace Upskill.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IByteSerializer _byteSerializer;
        private readonly AsyncLazy<IDatabase> _lazyCache;

        public CacheService(ICacheProvider cacheProvider, IByteSerializer byteSerializer)
        {
            _byteSerializer = byteSerializer;
            _lazyCache = new AsyncLazy<IDatabase>(cacheProvider.Get);
        }

        public async Task<IDataResult<T>> Get<T>(string key)
        {
            var cache = await _lazyCache;
            var resultFromCache = await cache.StringGetAsync(key);

            if (!resultFromCache.HasValue)
            {
                return new FailedDataResult<T>();
            }

            var result = _byteSerializer.FromByteArray<T>(resultFromCache);

            return new SuccessfulDataResult<T>(result);
        }

        public async Task Set<T>(string key, T value)
        {
            var cache = await _lazyCache;
            var bytes = _byteSerializer.ToByteArray(value);

            await cache.StringSetAsync(key, bytes);
        }

        public async Task Delete(string key)
        {
            var cache = await _lazyCache;
            await cache.KeyDeleteAsync(key);
        }
    }
}