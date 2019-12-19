using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Upskill.Storage.Options;

namespace Upskill.Storage.Providers
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
