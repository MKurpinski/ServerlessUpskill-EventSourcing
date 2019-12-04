using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Api.Utils
{
    public interface IFileToByteArrayConverter
    {
        Task<byte[]> Convert(IFormFile formFile);
    }
}
