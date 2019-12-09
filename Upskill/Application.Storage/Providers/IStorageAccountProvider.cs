using Microsoft.WindowsAzure.Storage;

namespace Application.Storage.Providers
{
    public interface IStorageAccountProvider
    {
        CloudStorageAccount Get();
    }
}