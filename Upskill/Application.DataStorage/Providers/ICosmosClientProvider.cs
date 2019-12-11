using Microsoft.Azure.Cosmos;

namespace Application.DataStorage.Providers
{
    public interface ICosmosClientProvider
    {
        CosmosClient Get();
    }
}
