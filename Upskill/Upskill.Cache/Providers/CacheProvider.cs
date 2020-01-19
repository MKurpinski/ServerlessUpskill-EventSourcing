using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Upskill.Cache.Options;

namespace Upskill.Cache.Providers
{
    public class CacheProvider : ICacheProvider
    {
        private readonly CacheOptions _cacheOptions;

        public CacheProvider(IOptions<CacheOptions> cacheOptionsAccessor)
        {
            _cacheOptions = cacheOptionsAccessor.Value;
        }

        public async Task<IDatabase> Get()
        {
            var connection = await ConnectionMultiplexer.ConnectAsync(_cacheOptions.ConnectionString);
            return connection.GetDatabase();
        }
    }
}