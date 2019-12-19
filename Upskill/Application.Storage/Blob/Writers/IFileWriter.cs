using System.Threading.Tasks;
using Upskill.Results;

namespace Application.Storage.Blob.Writers
{
    public interface IFileWriter
    {
        Task<IDataResult<string>> Write(
            string containerName,
            byte[] content,
            string contentType,
            string fileName);
    }
}
