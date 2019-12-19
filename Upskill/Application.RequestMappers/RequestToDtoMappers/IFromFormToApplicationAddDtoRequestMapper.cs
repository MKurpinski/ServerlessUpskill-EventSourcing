using System.Threading;
using System.Threading.Tasks;
using Application.RequestMappers.Dtos;
using Microsoft.AspNetCore.Http;
using Upskill.Results;

namespace Application.RequestMappers.RequestToDtoMappers
{
    public interface IFromFormToApplicationAddDtoRequestMapper
    {
        Task<IDataResult<RegisterApplicationDto>> MapRequest(
            HttpRequest request,
            CancellationToken cancellationToken = default);
    }
}
