using System.Threading.Tasks;

namespace Application.Storage.Blobs.Deleters
{
    public interface IFileDeleter
    {
        Task Delete(string containerName, string fileName);
    }
}
