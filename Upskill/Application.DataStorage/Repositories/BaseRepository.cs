using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DataStorage.Options;
using Application.DataStorage.Providers;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace Application.DataStorage.Repositories
{
    public abstract class BaseRepository<T>
    {
        private readonly IContainerClientProvider _containerClientProvider;
        private readonly DataStorageOptions _storageOptions;

        protected abstract string ContainerId { get; }
        protected abstract string PartitionKeyPath { get; }

        protected BaseRepository(
            IContainerClientProvider containerClientProvider,
            IOptions<DataStorageOptions> optionsAccessor)
        {
            _containerClientProvider = containerClientProvider;
            _storageOptions = optionsAccessor.Value;
        }

        protected async Task<Container> GetClient()
        {
            return await _containerClientProvider.Get(_storageOptions.DatabaseId, ContainerId, PartitionKeyPath);
        }

        public async Task<IReadOnlyCollection<T>> GetByField(string field, string value)
        {
            var container = await this.GetClient();

            var query = new QueryDefinition($"SELECT * FROM {ContainerId} a WHERE a.{field} = @value")
                .WithParameter("@value", value);

            var results = new List<T>();
            var resultSetIterator = container.GetItemQueryIterator<T>(query, requestOptions: new QueryRequestOptions());

            while (resultSetIterator.HasMoreResults)
            {
                var response = await resultSetIterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }
    }
}
