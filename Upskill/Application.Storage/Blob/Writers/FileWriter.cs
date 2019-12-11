using System;
using System.Threading.Tasks;
using Application.Results;
using Application.Results.Implementation;
using Application.Storage.Blob.Providers;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Application.Storage.Blob.Writers
{
    public class FileWriter : IFileWriter
    {
        private readonly CloudBlobClient _blobClient;

        public FileWriter(IBlobClientProvider blobClientProvider)
        {
            _blobClient = blobClientProvider.Get();
        }

        public async Task<IDataResult<string>> Write(string containerName, byte[] content, string contentType, string fileName)
        {
            try
            {
                var container = _blobClient.GetContainerReference(containerName);
                await container.CreateIfNotExistsAsync();

                var blob = container.GetBlockBlobReference(fileName);
                blob.Properties.ContentType = contentType;

                await blob.UploadFromByteArrayAsync(content, 0, content.Length);

                return new SuccessfulDataResult<string>(blob.Uri.AbsoluteUri);
            }
            catch (Exception ex)
            {
                return new FailedDataResult<string>(nameof(Write), $"{ex.Message} \n{ex.StackTrace}");
            }
        }
    }
}
