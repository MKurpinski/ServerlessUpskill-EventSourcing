using System.Linq;
using System.Threading.Tasks;
using Application.Commands.Commands;
using Application.Commands.Commands.Candidate;
using Application.Commands.Utils;
using Application.RequestMappers.Dtos;
using AutoMapper;
using HeyRed.Mime;
using Microsoft.AspNetCore.Http;
using Upskill.Infrastructure;

namespace Application.Commands.CommandBuilders
{
    public class RegisterApplicationCommandBuilder : ICommandBuilder<RegisterApplicationDto, RegisterApplicationCommand>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IFileToByteArrayConverter _fileToByteArrayConverter;
        private readonly IMapper _mapper;

        public RegisterApplicationCommandBuilder(
            IDateTimeProvider dateTimeProvider,
            IFileToByteArrayConverter fileToByteArrayConverter,
            IMapper mapper)
        {
            _dateTimeProvider = dateTimeProvider;
            _fileToByteArrayConverter = fileToByteArrayConverter;
            _mapper = mapper;
        }

        public async Task<RegisterApplicationCommand> Build(RegisterApplicationDto from)
        {
            var creationTime = _dateTimeProvider.GetCurrentDateTime();
            var cv = await this.BuildApplicationFile(from.Cv);
            var photo = await this.BuildApplicationFile(from.Photo);

            var candidate = _mapper.Map<CandidateDto, Candidate>(from.Candidate);

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
