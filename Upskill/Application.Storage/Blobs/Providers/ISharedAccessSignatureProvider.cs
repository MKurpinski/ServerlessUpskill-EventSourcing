using System;

namespace Application.Storage.Blobs.Providers
{
    public interface ISharedAccessSignatureProvider
    {
        string GetContainerSasUri(string containerName, TimeSpan validFor);
    }
}
