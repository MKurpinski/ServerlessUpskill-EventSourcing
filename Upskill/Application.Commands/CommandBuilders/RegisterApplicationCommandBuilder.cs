using System.Threading.Tasks;
using Application.Commands.Commands;
using Application.Commands.Utils;
using Application.Infrastructure;
using Application.RequestMappers.Dtos;
using HeyRed.Mime;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.CommandBuilders
{
    public class RegisterApplicationCommandBuilder : ICommandBuilder<RegisterApplicationDto, RegisterApplicationCommand>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IFileToByteArrayConverter _fileToByteArrayConverter;

        public RegisterApplicationCommandBuilder(
            IDateTimeProvider dateTimeProvider,
            IFileToByteArrayConverter fileToByteArrayConverter)
        {
            _dateTimeProvider = dateTimeProvider;
            _fileToByteArrayConverter = fileToByteArrayConverter;
        }

        public async Task<RegisterApplicationCommand> Build(RegisterApplicationDto from)
        {
            var creationTime = _dateTimeProvider.GetCurrentDateTime();
            var cv = await this.BuildApplicationFile(from.Cv);
            var photo = await this.BuildApplicationFile(from.Photo);
            var candidate = new Candidate(from.Candidate.FirstName, from.Candidate.LastName,from.Candidate.Category);

            var command = new RegisterApplicationCommand(cv, photo, candidate, creationTime);
            return command;
        }

        private async Task<ApplicationFile> BuildApplicationFile(IFormFile file)
        {
            var content = await _fileToByteArrayConverter.Convert(file);
            var contentType = file.ContentType;
            var extension = MimeTypesMap.GetExtension(file.ContentType);
            return new ApplicationFile(content, contentType, extension);
        }
    }
}
