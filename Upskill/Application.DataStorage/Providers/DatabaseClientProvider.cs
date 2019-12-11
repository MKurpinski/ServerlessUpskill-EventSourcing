using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Application.DataStorage.Providers
{
    public class DatabaseClientProvider: IDatabaseClientProvider
    {
        private readonly Lazy<CosmosClient> _lazyCosmosClient;

        public DatabaseClientProvider(ICosmosClientProvider cosmosClientProvider)
        {
            _lazyCosmosClient = new Lazy<CosmosClient>(cosmosClientProvider.Get);
        }

        public async Task<Database> Get(string databaseId)
        {
            await _lazyCosmosClient.Value.CreateDatabaseIfNotExistsAsync(databaseId);
            return _lazyCosmosClient.Value.GetDatabase(databaseId);
        }
    }
}
