using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Api.Dtos;
using Application.Api.RequestToDtoMappers.Extensions;
using Application.Api.RequestToDtoMappers.Results;
using Application.Api.RequestToDtoMappers.Results.Implementation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Application.Api.RequestToDtoMappers.Implementation
{
    public class FromFormToApplicationDtoDeserializer : IFromFormToApplicationDtoDeserializer
    {

        public async Task<IResult<RegisterApplicationDto>> HandleDeserializationFromForm(HttpRequest request, CancellationToken cancellationToken)
        {
            var formData = await request.ReadFormAsync(cancellationToken);

            var candidateDeserializationResult = this.ProvideCandidate(formData);
            var cvDeserializationResult = this.ProvideFile(formData, nameof(RegisterApplicationDto.Cv));
            var photoDeserializationResult = this.ProvideFile(formData, nameof(RegisterApplicationDto.Photo));

            if (!candidateDeserializationResult.Success || !cvDeserializationResult.Success || !photoDeserializationResult.Success)
            {
                var errors = new[] {candidateDeserializationResult.Errors, cvDeserializationResult.Errors, photoDeserializationResult.Errors};
                return new FailedResult<RegisterApplicationDto>(errors.SelectMany(x => x));
            }

            var dto = new RegisterApplicationDto(
                cvDeserializationResult.Value,
                photoDeserializationResult.Value,
                candidateDeserializationResult.Value
            );

            return new SuccessfulResult<RegisterApplicationDto>(dto);
        }

        private IResult<IFormFile> ProvideFile(IFormCollection form, string key)
        {
            var containFile = form.TryGetFile(key, out var file);
            if (!containFile)
            {
                return new FailedResult<IFormFile>(key, "File is required");
            }

            return new SuccessfulResult<IFormFile>(file);
        }

        private IResult<CandidateDto> ProvideCandidate(IFormCollection form)
        {
            var containCandidate = form.TryGetValue(
                nameof(RegisterApplicationDto.Candidate),
                StringComparison.OrdinalIgnoreCase,
                out var candidateAsString);

            if (!containCandidate)
            {
                return new FailedResult<CandidateDto>(nameof(RegisterApplicationDto.Candidate), "Candidate info is required");
            }

            var deserializedCandidate = JsonConvert.DeserializeObject<CandidateDto>(candidateAsString);
            return new SuccessfulResult<CandidateDto>(deserializedCandidate);
        }
    }
}
