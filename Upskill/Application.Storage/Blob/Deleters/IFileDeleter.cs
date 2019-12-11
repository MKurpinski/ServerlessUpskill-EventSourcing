using System.Threading.Tasks;

namespace Application.Storage.Blob.Deleters
{
    public interface IFileDeleter
    {
        Task Delete(string containerName, string fileName);
    }
}
