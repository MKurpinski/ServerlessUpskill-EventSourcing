using Microsoft.WindowsAzure.Storage;

namespace Upskill.Storage.Providers
{
    public interface IStorageAccountProvider
    {
        CloudStorageAccount Get();
    }
}