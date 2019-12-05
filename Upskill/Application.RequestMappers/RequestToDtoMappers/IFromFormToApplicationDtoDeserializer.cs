using System.Threading;
using System.Threading.Tasks;
using Application.RequestMappers.Dtos;
using Application.RequestMappers.RequestToDtoMappers.Results;
using Microsoft.AspNetCore.Http;

namespace Application.RequestMappers.RequestToDtoMappers
{
    public interface IFromFormToApplicationDtoDeserializer
    {
        Task<IResult<RegisterApplicationDto>> HandleDeserializationFromForm(HttpRequest request,
            CancellationToken cancellationToken);
    }
}
