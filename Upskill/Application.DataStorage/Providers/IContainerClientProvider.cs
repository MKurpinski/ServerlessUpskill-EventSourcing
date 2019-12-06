using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Application.DataStorage.Providers
{
    public interface IContainerClientProvider
    {
        Task<Container> Get(string databaseId, string containerId, string partitionKey);
    }
}
