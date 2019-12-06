using Application.DataStorage.Options;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Options;

namespace Application.DataStorage.Providers
{
    public class CosmosClientProvider : ICosmosClientProvider
    {
        private readonly DataStorageOptions _storageOptions;
        public CosmosClientProvider(IOptions<DataStorageOptions> dataStorageOptionsAccessor)
        {
            _storageOptions = dataStorageOptionsAccessor.Value;
        }

        public CosmosClient Get()
        {
            return new CosmosClientBuilder(_storageOptions.DataStorageConnectionString)
                .WithSerializerOptions(new CosmosSerializationOptions{PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase})
                .Build();
        }
    }
}
