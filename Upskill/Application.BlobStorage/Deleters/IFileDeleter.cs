using System.Threading.Tasks;

namespace Application.BlobStorage.Deleters
{
    public interface IFileDeleter
    {
        Task Delete(string containerName, string fileName);
    }
}
