using System.Threading.Tasks;
using Application.Storage.Providers;
using Microsoft.WindowsAzure.Storage.Table;

namespace Application.Storage.Table.Providers
{
    public class TableClientProvider : ITableClientProvider
    {
        private readonly IStorageAccountProvider _storageAccountProvider;

        public TableClientProvider(IStorageAccountProvider storageAccountProvider)
        {
            _storageAccountProvider = storageAccountProvider;
        }

        public async Task<CloudTable> Get(string tableName)
        {
            var storageAccount = _storageAccountProvider.Get();

            var tableClient = storageAccount.CreateCloudTableClient();

            var table = tableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}
