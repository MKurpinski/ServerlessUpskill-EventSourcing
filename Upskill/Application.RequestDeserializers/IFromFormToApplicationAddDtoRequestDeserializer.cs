using System.Threading;
using System.Threading.Tasks;
using Application.RequestDeserializers.Dtos;
using Application.RequestDeserializers.Results;
using Microsoft.AspNetCore.Http;

namespace Application.RequestDeserializers
{
    public interface IFromFormToApplicationAddDtoRequestDeserializer
    {
        Task<IDeserializationResult<AddApplicationFormDto>> Deserialize(
            HttpRequest request,
            CancellationToken cancellationToken = default);
    }
}
