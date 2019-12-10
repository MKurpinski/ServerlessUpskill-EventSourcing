using Application.Storage.Options;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;

namespace Application.Storage.Providers
{
    public class StorageAccountProvider : IStorageAccountProvider
    {
        private readonly StorageOptions _options;
        public StorageAccountProvider(IOptions<StorageOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }
        public CloudStorageAccount Get()
        {
            var connectionString = _options.ConnectionString;
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            return storageAccount;
        }
    }
}
