using System;
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
        private const int MAX_LENGTH_OF_CV_IN_MEGABYTES = 10;
        private const int MAX_LENGTH_OF_PHOTO_IN_MEGABYTES = 2;
        private static readonly string[] ALLOWED_CV_FORMATS = {"pdf"};
        private static readonly string[] ALLOWED_PHOTO_FORMATS = {"png", "jpg"};
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
                    MAX_LENGTH_OF_CV_IN_MEGABYTES,
                    ALLOWED_CV_FORMATS));

            RuleFor(x => x.Photo)
                .NotNull()
                .Custom((file, context) => FileValidation(
                    nameof(RegisterApplicationDto.Photo),
                    file,
                    context,
                    MAX_LENGTH_OF_PHOTO_IN_MEGABYTES,
                    ALLOWED_PHOTO_FORMATS));

            RuleFor(x => x.Candidate).NotNull().SetValidator(candidateDtoValidator);
        }

        private void FileValidation(string propertyName, IFormFile file, CustomContext context, int maxFileLengthInMb, string[] allowedExtensions)
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