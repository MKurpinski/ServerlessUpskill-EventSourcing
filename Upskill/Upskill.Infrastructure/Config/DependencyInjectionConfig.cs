using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Infrastructure.Implementation;

namespace Upskill.Infrastructure.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddInfrastructureModule(this IFunctionsHostBuilder builder)
        {
            return builder.Services
                .AddTransient<IGuidProvider, GuidProvider>()
                .AddTransient<IDateTimeProvider, DateTimeProvider>();
        }
    }
}
