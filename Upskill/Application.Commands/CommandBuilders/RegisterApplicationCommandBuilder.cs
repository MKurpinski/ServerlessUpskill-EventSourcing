using System.Linq;
using System.Threading.Tasks;
using Application.Commands.Commands;
using Application.Commands.Commands.Candidate;
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

            var address = new Address(from.Candidate.Address.City, from.Candidate.Address.Country);

            var finishedSchools =
                from.Candidate.FinishedSchools?.Select(x => new FinishedSchool(x.Name, x.StartDate, x.FinishDate))
                ?? Enumerable.Empty<FinishedSchool>();

            var confirmedSkills =
                from.Candidate.ConfirmedSkills?.Select(x =>
                    new ConfirmedSkill(x.Name, x.DateOfAchievement))
                ?? Enumerable.Empty<ConfirmedSkill>();

            var workExperience =
                from.Candidate.WorkExperiences?.Select(x =>
                    new WorkExperience(x.CompanyName, x.Position, x.StartDate, x.FinishDate))
                ?? Enumerable.Empty<WorkExperience>();

            var candidate = new Candidate(
                from.Candidate.FirstName,
                from.Candidate.LastName,
                from.Candidate.Category,
                from.Candidate.EducationLevel,
                address,
                finishedSchools.ToList(),
                confirmedSkills.ToList(),
                workExperience.ToList());

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
