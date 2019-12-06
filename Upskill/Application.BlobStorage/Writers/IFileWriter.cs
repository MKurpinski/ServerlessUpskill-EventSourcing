using System.Threading.Tasks;

namespace Application.BlobStorage.Writers
{
    public interface IFileWriter
    {
        Task<string> Write(
            string containerName,
            byte[] content,
            string contentType,
            string fileName,
            string extension);
    }
}
