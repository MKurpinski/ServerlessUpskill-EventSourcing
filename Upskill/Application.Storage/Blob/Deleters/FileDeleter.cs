using System.Threading.Tasks;
using Application.Storage.Blob.Providers;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Application.Storage.Blob.Deleters
{
    public class FileDeleter : IFileDeleter
    {
        private readonly CloudBlobClient _blobClient;

        public FileDeleter(IBlobClientProvider blobClientProvider)
        {
            _blobClient = blobClientProvider.Get();
        }

        public async Task Delete(string containerName, string fileName)
        {
            var container = _blobClient.GetContainerReference(containerName);
            var containerExists = await container.ExistsAsync();
            
            if (!containerExists)
            {
                return;
            }

            var blob = container.GetBlockBlobReference(fileName);
            await blob.DeleteIfExistsAsync();
        }
    }
}
