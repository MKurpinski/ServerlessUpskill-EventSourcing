using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Utils.Implementation
{
    public class FileToByteArrayConverter : IFileToByteArrayConverter
    {
        public async Task<byte[]> Convert(IFormFile formFile)
        {
            using (var ms = new MemoryStream())
            {
                await formFile.CopyToAsync(ms);
                return ms.ToArray();
            }
        }
    }
}