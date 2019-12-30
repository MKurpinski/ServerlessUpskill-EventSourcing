using System;
using Microsoft.WindowsAzure.Storage.Blob;
using Upskill.Storage.Blobs;

namespace Application.Storage.Blobs.Providers
{
    public class SharedAccessSignatureProvider : ISharedAccessSignatureProvider
    {
        private readonly CloudBlobClient _blobClient;

        public SharedAccessSignatureProvider(IBlobClientProvider blobClientProvider)
        {
            _blobClient = blobClientProvider.Get();
        }

        public string GetContainerSasUri(string containerName, TimeSpan validFor)
        {
            var adHocPolicy = new SharedAccessBlobPolicy
            {
                SharedAccessExpiryTime = DateTime.UtcNow.Add(validFor),
                Permissions = SharedAccessBlobPermissions.Read
            };

            var container = _blobClient.GetContainerReference(containerName);
            var sasContainerToken = container.GetSharedAccessSignature(adHocPolicy, null);

            return sasContainerToken;
        }
    }
}