using System.Threading;
using System.Threading.Tasks;
using Application.RequestMappers.Dtos;
using Application.Results;
using Microsoft.AspNetCore.Http;

namespace Application.RequestMappers.RequestToDtoMappers
{
    public interface IFromFormToApplicationDtoDeserializer
    {
        Task<IDataResult<RegisterApplicationDto>> HandleDeserializationFromForm(HttpRequest request,
            CancellationToken cancellationToken);
    }
}
