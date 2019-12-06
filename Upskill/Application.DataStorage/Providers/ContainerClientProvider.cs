using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Application.DataStorage.Providers
{
    public class ContainerClientProvider : IContainerClientProvider
    {
        private readonly IDatabaseClientProvider _databaseClientProvider;

        public ContainerClientProvider(IDatabaseClientProvider databaseClientProvider)
        {
            _databaseClientProvider = databaseClientProvider;
        }

        public async Task<Container> Get(string databaseId, string containerId, string partitionKey)
        {
            var databaseClient = await _databaseClientProvider.Get(databaseId);
            var containerProperties = new ContainerProperties(containerId, $"/{partitionKey}");
            await databaseClient.CreateContainerIfNotExistsAsync(containerProperties);
            var container = databaseClient.GetContainer(containerId);
            return container;
        }
    }
}
