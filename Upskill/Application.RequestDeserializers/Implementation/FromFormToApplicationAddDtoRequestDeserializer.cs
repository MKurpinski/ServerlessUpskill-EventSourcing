using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.RequestDeserializers.Dtos;
using Application.RequestDeserializers.Extensions;
using Application.RequestDeserializers.Results;
using Application.RequestDeserializers.Results.Implementation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Application.RequestDeserializers.Implementation
{
    public class FromFormToApplicationAddDtoRequestDeserializer: IFromFormToApplicationAddDtoRequestDeserializer
    {
        public async Task<IDeserializationResult<AddApplicationFormDto>> Deserialize(
            HttpRequest request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var formData = await request.ReadFormAsync(cancellationToken);

                var candidateDeserializationResult = this.ProvideCandidate(formData);
                var cvDeserializationResult = this.ProvideFile(formData, nameof(AddApplicationFormDto.Cv));
                var photoDeserializationResult = this.ProvideFile(formData, nameof(AddApplicationFormDto.Photo));

                if (!candidateDeserializationResult.Success || !cvDeserializationResult.Success ||
                    !photoDeserializationResult.Success)
                {
                    return new FailedDeserializationResult<AddApplicationFormDto>();
                }

                var dto = new AddApplicationFormDto(
                    cvDeserializationResult.Value,
                    photoDeserializationResult.Value,
                    candidateDeserializationResult.Value
                );

                return new SuccessfulDeserializationResult<AddApplicationFormDto>(dto);
            }
            catch (Exception)
            {
                return new FailedDeserializationResult<AddApplicationFormDto>();
            }
        }

        private IDeserializationResult<byte[]> ProvideFile(IFormCollection form, string key)
        {
            var containFile = form.TryGetFile(key, out var file);
            if (!containFile)
            {
                return new FailedDeserializationResult<byte[]>();
            }

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();

                return new SuccessfulDeserializationResult<byte[]>(fileBytes);
            }
        }

        private IDeserializationResult<CandidateDto> ProvideCandidate(IFormCollection form)
        {
            var containCandidate = form.TryGetValue(
                nameof(AddApplicationFormDto.Candidate),
                StringComparison.OrdinalIgnoreCase,
                out var candidateAsString);

            if (!containCandidate)
            {
                return new FailedDeserializationResult<CandidateDto>();
            }

            var deserializedCandidate = JsonConvert.DeserializeObject<CandidateDto>(candidateAsString);
            return new SuccessfulDeserializationResult<CandidateDto>(deserializedCandidate);
        }
    }
}
