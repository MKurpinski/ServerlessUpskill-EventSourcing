using System.Threading;
using System.Threading.Tasks;
using Application.RequestMappers.Dtos;
using Microsoft.AspNetCore.Http;
using Upskill.Results;

namespace Application.RequestMappers.RequestToDtoMappers
{
    public interface IFromFormToApplicationDtoDeserializer
    {
        Task<IDataResult<RegisterApplicationDto>> HandleDeserializationFromForm(HttpRequest request,
            CancellationToken cancellationToken);
    }
}
