using System.Threading;
using System.Threading.Tasks;
using Application.RequestMappers.Dtos;
using Application.RequestMappers.RequestToDtoMappers.Results;
using Microsoft.AspNetCore.Http;

namespace Application.RequestMappers.RequestToDtoMappers
{
    public interface IFromFormToApplicationAddDtoRequestMapper
    {
        Task<IResult<RegisterApplicationDto>> MapRequest(
            HttpRequest request,
            CancellationToken cancellationToken = default);
    }
}
