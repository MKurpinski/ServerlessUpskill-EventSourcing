using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Utils
{
    public interface IFileToByteArrayConverter
    {
        Task<byte[]> Convert(IFormFile formFile);
    }
}
