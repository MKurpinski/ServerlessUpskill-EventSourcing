using System.Threading;
using System.Threading.Tasks;
using Application.Api.Dtos;
using Application.Api.RequestToDtoMappers.Results;
using Microsoft.AspNetCore.Http;

namespace Application.Api.RequestToDtoMappers
{
    public interface IFromFormToApplicationDtoDeserializer
    {
        Task<IResult<RegisterApplicationDto>> HandleDeserializationFromForm(HttpRequest request,
            CancellationToken cancellationToken);
    }
}
