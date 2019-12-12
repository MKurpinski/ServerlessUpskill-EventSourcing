using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.RequestMappers.Dtos;
using Application.RequestMappers.RequestToDtoMappers.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Upskill.Results;
using Upskill.Results.Implementation;

namespace Application.RequestMappers.RequestToDtoMappers.Implementation
{
    public class FromFormToApplicationDtoDeserializer : IFromFormToApplicationDtoDeserializer
    {

        public async Task<IDataResult<RegisterApplicationDto>> HandleDeserializationFromForm(HttpRequest request, CancellationToken cancellationToken)
        {
            var formData = await request.ReadFormAsync(cancellationToken);

            var candidateDeserializationResult = this.ProvideCandidate(formData);
            var cvDeserializationResult = this.ProvideFile(formData, nameof(RegisterApplicationDto.Cv));
            var photoDeserializationResult = this.ProvideFile(formData, nameof(RegisterApplicationDto.Photo));

            if (!candidateDeserializationResult.Success || !cvDeserializationResult.Success || !photoDeserializationResult.Success)
            {
                var errors = new[] {candidateDeserializationResult.Errors, cvDeserializationResult.Errors, photoDeserializationResult.Errors};
                return new FailedDataResult<RegisterApplicationDto>(errors.SelectMany(x => x));
            }

            var dto = new RegisterApplicationDto(
                cvDeserializationResult.Value,
                photoDeserializationResult.Value,
                candidateDeserializationResult.Value
            );

            return new SuccessfulDataResult<RegisterApplicationDto>(dto);
        }

        private IDataResult<IFormFile> ProvideFile(IFormCollection form, string key)
        {
            var containFile = form.TryGetFile(key, out var file);
            if (!containFile)
            {
                return new FailedDataResult<IFormFile>(key, "File is required");
            }

            return new SuccessfulDataResult<IFormFile>(file);
        }

        private IDataResult<CandidateDto> ProvideCandidate(IFormCollection form)
        {
            var containCandidate = form.TryGetValue(
                nameof(RegisterApplicationDto.Candidate),
                StringComparison.OrdinalIgnoreCase,
                out var candidateAsString);

            if (!containCandidate)
            {
                return new FailedDataResult<CandidateDto>(nameof(RegisterApplicationDto.Candidate), "Candidate info is required");
            }

            var deserializedCandidate = JsonConvert.DeserializeObject<CandidateDto>(candidateAsString);
            return new SuccessfulDataResult<CandidateDto>(deserializedCandidate);
        }
    }
}
