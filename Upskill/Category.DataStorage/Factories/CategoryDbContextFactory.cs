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
            const string fakeConnectionString = "Server=localhost;Database=Upskill;Uid=user;Pwd=password;";
            var connectionStrings = Environment.GetEnvironmentVariable(DataStorageConnections.SqlConnectionString) ?? fakeConnectionString;
            var optionsBuilder = new DbContextOptionsBuilder<CategoryDbContext>();
            optionsBuilder.UseMySql(connectionStrings);

            return new CategoryDbContext(optionsBuilder.Options);
        }
    }
}
