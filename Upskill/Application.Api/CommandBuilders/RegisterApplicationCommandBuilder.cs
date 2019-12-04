using System.Threading.Tasks;
using Application.Api.Commands.RegisterApplicationCommand;
using Application.Api.Dtos;
using Application.Api.Utils;
using Application.Infrastructure;

namespace Application.Api.CommandBuilders
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
            var cv = await _fileToByteArrayConverter.Convert(from.Cv);
            var photo = await _fileToByteArrayConverter.Convert(from.Photo);
            var candidate = new Candidate(from.Candidate.FirstName, from.Candidate.LastName);

            var command = new RegisterApplicationCommand(cv, photo, candidate, creationTime);
            return command;
        }
    }
}
