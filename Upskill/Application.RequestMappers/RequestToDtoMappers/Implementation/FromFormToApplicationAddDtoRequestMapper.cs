using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.RequestMappers.Dtos;
using Application.Results;
using Application.Results.Implementation;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.RequestMappers.RequestToDtoMappers.Implementation
{
    public class FromFormToApplicationAddDtoRequestMapper: IFromFormToApplicationAddDtoRequestMapper
    {
        private readonly IValidator<RegisterApplicationDto> _registerApplicationDtoValidator;
        private readonly IFromFormToApplicationDtoDeserializer _fromFormToApplicationDtoDeserializer;

        public FromFormToApplicationAddDtoRequestMapper(
            IValidator<RegisterApplicationDto> registerApplicationDtoValidator,
            IFromFormToApplicationDtoDeserializer fromFormToApplicationDtoDeserializer)
        {
            _registerApplicationDtoValidator = registerApplicationDtoValidator;
            _fromFormToApplicationDtoDeserializer = fromFormToApplicationDtoDeserializer;
        }

        public async Task<IDataResult<RegisterApplicationDto>> MapRequest(
            HttpRequest request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var deserializationFromFormResult = await _fromFormToApplicationDtoDeserializer.HandleDeserializationFromForm(request, cancellationToken);
                if (!deserializationFromFormResult.Success)
                {
                    return deserializationFromFormResult;
                }

                var validationResult = await _registerApplicationDtoValidator.ValidateAsync(deserializationFromFormResult.Value, cancellationToken);

                if (!validationResult.IsValid)
                {
                    var preparedErrors = validationResult.Errors.Select(x => new KeyValuePair<string, string>(x.PropertyName, x.ErrorMessage));
                    return  new FailedDataResult<RegisterApplicationDto>(preparedErrors);
                }

                return new SuccessfulDataResult<RegisterApplicationDto>(deserializationFromFormResult.Value);
            }
            catch (Exception)
            {
                return new FailedDataResult<RegisterApplicationDto>("InvalidData", "Data in invalid format");
            }
        }
    }
}
