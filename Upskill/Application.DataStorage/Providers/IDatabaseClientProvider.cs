using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Application.DataStorage.Providers
{
    public interface IDatabaseClientProvider
    {
        Task<Database> Get(string databaseName);
    }
}
