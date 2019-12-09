using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.ProcessStatus.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddProcessStatusModule(this IFunctionsHostBuilder builder)
        {
            return builder.Services
                .AddTransient<IProcessStatusHandler, ProcessStatusHandler>();
        }
    }
}
