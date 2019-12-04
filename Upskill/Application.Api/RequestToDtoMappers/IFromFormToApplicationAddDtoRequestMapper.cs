using System.Threading;
using System.Threading.Tasks;
using Application.Api.Dtos;
using Application.Api.RequestToDtoMappers.Results;
using Microsoft.AspNetCore.Http;

namespace Application.Api.RequestToDtoMappers
{
    public interface IFromFormToApplicationAddDtoRequestMapper
    {
        Task<IResult<RegisterApplicationDto>> Deserialize(
            HttpRequest request,
            CancellationToken cancellationToken = default);
    }
}
