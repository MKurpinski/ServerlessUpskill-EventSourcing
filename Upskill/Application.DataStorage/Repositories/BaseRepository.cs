using System.Threading.Tasks;
using Application.DataStorage.Options;
using Application.DataStorage.Providers;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace Application.DataStorage.Repositories
{
    public abstract class BaseRepository
    {
        private readonly IContainerClientProvider _containerClientProvider;
        private readonly DataStorageOptions _storageOptions;

        protected abstract string ContainerId { get; }
        protected abstract string PartitionKey { get; }

        protected BaseRepository(
            IContainerClientProvider containerClientProvider,
            IOptions<DataStorageOptions> optionsAccessor)
        {
            _containerClientProvider = containerClientProvider;
            _storageOptions = optionsAccessor.Value;
        }

        protected async Task<Container> GetClient()
        {
            return await _containerClientProvider.Get(_storageOptions.DatabaseId, ContainerId, PartitionKey);
        }
    }
}
