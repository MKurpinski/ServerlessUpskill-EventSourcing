using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Search.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddSearchModule(this IFunctionsHostBuilder builder)
        {
            return null;
        }
    }
}
