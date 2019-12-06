using System.Threading.Tasks;

namespace Application.BlobStorage
{
    public interface IFileDeleter
    {
        Task Delete(string containerName, string fileName, string extension);
    }
}
