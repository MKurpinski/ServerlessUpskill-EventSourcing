using System;
using System.Collections.Generic;
using System.Linq;
using Application.RequestMappers.Dtos;
using Application.RequestMappers.Options;
using FluentValidation;
using FluentValidation.Validators;
using HeyRed.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Application.RequestMappers.Validators
{
    public class RegisterApplicationDtoValidator: AbstractValidator<RegisterApplicationDto>
    {
        public RegisterApplicationDtoValidator(
            IOptions<ApplicationFormValidationOptions> applicationFormOptionsAccessor,
            IValidator<CandidateDto> candidateDtoValidator)
        {
            var applicationFormOptions = applicationFormOptionsAccessor.Value;
            RuleFor(x => x.Cv)
                .NotNull()
                .Custom((file, context) => FileValidation(
                    nameof(RegisterApplicationDto.Cv),
                    file,
                    context,
                    applicationFormOptions.MaxCvSizeInMegabytes,
                    applicationFormOptions.CvFormats));

            RuleFor(x => x.Photo)
                .NotNull()
                .Custom((file, context) => FileValidation(
                    nameof(RegisterApplicationDto.Photo),
                    file,
                    context,
                    applicationFormOptions.MaxPhotoSizeInMegabytes,
                    applicationFormOptions.PhotoFormats));

            RuleFor(x => x.Candidate).NotNull().SetValidator(candidateDtoValidator);
        }

        private void FileValidation(string propertyName, IFormFile file, CustomContext context, int maxFileLengthInMb, IEnumerable<string> allowedExtensions)
        {
            const int bytesInMegaByte = 1000000;
            var maxLengthInBytes = maxFileLengthInMb * bytesInMegaByte;

            if (file.Length == 0)
            {
                context.AddFailure(propertyName, "File cannot be empty");
                return;
            }

            if (!allowedExtensions.Any(extension => extension.Equals(MimeTypesMap.GetExtension(file.ContentType), StringComparison.OrdinalIgnoreCase)))
            {
                context.AddFailure(propertyName, "Not supported file type");
                return;
            }

            if (file.Length > maxLengthInBytes)
            {
                context.AddFailure(propertyName, "Too big file");
            }
        }
    }
}