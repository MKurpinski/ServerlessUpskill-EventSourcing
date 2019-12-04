﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Api.Dtos;
using Application.Api.RequestToDtoMappers.Results;
using Application.Api.RequestToDtoMappers.Results.Implementation;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Api.RequestToDtoMappers.Implementation
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

        public async Task<IResult<RegisterApplicationDto>> Deserialize(
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
                    return  new FailedResult<RegisterApplicationDto>(preparedErrors);
                }

                return new SuccessfulResult<RegisterApplicationDto>(deserializationFromFormResult.Value);
            }
            catch (Exception)
            {
                return new FailedResult<RegisterApplicationDto>("InvalidData", "Data in invalid format");
            }
        }
    }
}
