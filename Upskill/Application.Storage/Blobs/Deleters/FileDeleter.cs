using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Upskill.Storage.Blobs;

namespace Application.Storage.Blobs.Deleters
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
