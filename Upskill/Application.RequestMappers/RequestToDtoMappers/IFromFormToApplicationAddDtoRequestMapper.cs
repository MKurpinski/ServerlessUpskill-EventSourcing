using System.Threading;
using System.Threading.Tasks;
using Application.RequestMappers.Dtos;
using Application.Results;
using Microsoft.AspNetCore.Http;

namespace Application.RequestMappers.RequestToDtoMappers
{
    public interface IFromFormToApplicationAddDtoRequestMapper
    {
        Task<IDataResult<RegisterApplicationDto>> MapRequest(
            HttpRequest request,
            CancellationToken cancellationToken = default);
    }
}
