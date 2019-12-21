using System;
using Category.DataStorage.Constants;
using Category.DataStorage.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Category.DataStorage.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDataStorageModule(this IFunctionsHostBuilder builder)
        {
            var mySqlConnectionString = Environment.GetEnvironmentVariable(DataStorageConnections.SqlConnectionString);

            builder.Services.AddDbContext<CategoryDbContext>(options =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.UseMySQL(mySqlConnectionString);
            });

            return builder.Services
                .AddTransient<ICategoryRepository, CategoryRepository>();
        }
    }
}
