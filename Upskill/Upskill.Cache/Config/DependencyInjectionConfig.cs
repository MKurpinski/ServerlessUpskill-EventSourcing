using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Cache.Options;
using Upskill.Cache.Providers;
using Upskill.Cache.Utils;
using Upskill.Infrastructure.Extensions;

namespace Upskill.Cache.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddCache(this IFunctionsHostBuilder builder)
        {
            builder.ConfigureOptions<CacheOptions>();

            return builder.Services
                .AddTransient<ICacheService, CacheService>()
                .AddTransient<IByteSerializer, ByteSerializer>()
                .AddTransient<ICacheProvider, CacheProvider>();
        }
    }
}
