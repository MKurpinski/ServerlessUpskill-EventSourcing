using System;
using Category.DataStorage.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Category.DataStorage.Factories
{
    public class CategoryDbContextFactory : IDesignTimeDbContextFactory<CategoryDbContext>
    {
        public CategoryDbContext CreateDbContext(string[] args)
        {
            var connectionStrings = Environment.GetEnvironmentVariable(DataStorageConnections.SqlConnectionString);
            var optionsBuilder = new DbContextOptionsBuilder<CategoryDbContext>();
            optionsBuilder.UseMySql(connectionStrings);

            return new CategoryDbContext(optionsBuilder.Options);
        }
    }
}
