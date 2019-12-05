using System.Threading.Tasks;

namespace Application.BlobStorage.Writers
{
    public interface IFileWriter
    {
        Task<string> Save(
            string containerName,
            byte[] content,
            string contentType,
            string fileName,
            string extension);
    }
}
