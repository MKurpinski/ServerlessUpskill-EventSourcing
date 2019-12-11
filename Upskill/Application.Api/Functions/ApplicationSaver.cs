using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Api.Events.Internal;
using Application.Commands.Commands;
using Application.DataStorage.Model;
using Application.DataStorage.Repositories;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Application.Api.Functions
{
    public class ApplicationSaver
    {
        private readonly IApplicationRepository _applicationRepository;

        public ApplicationSaver(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        [FunctionName(nameof(ApplicationSaver))]
        public async Task Run(
            [DurableClient] IDurableOrchestrationClient client,
            [ActivityTrigger] IDurableActivityContext context,
            ILogger log)
        {
            var command = context.GetInput<SaveApplicationCommand>();

            var address = new Address(command.Address.City, command.Address.Country);

            var finishedSchools =
                command.FinishedSchools?.Select(x => new FinishedSchool(x.Name, x.StartDate, x.FinishDate))
                ?? Enumerable.Empty<FinishedSchool>();

            var confirmedSkills =
                command.ConfirmedSkills?.Select(x =>
                    new ConfirmedSkill(x.Name, x.DateOfAchievement))
                ?? Enumerable.Empty<ConfirmedSkill>();

            var workExperience =
                command.WorkExperiences?.Select(x =>
                    new WorkExperience(x.CompanyName, x.Position, x.StartDate, x.FinishDate))
                ?? Enumerable.Empty<WorkExperience>();

            var applicationToSave = new DataStorage.Model.Application(
                context.InstanceId,
                command.CreationTime,
                command.PhotoId,
                command.CvId,
                command.Category,
                command.FirstName,
                command.LastName,
                command.EducationLevel,
                address,
                finishedSchools.ToList(),
                confirmedSkills.ToList(),
                workExperience.ToList());

            var saveResult = await _applicationRepository.Create(applicationToSave);

            if (!saveResult.Success)
            {
                this.LogError(log, context.InstanceId, string.Join(',', saveResult.Errors.Select(x => x.Value)));
                var eventToDispatch = new ApplicationSaveFailedEvent(saveResult.Errors);
                await client.RaiseEventAsync(context.InstanceId, nameof(ApplicationSaveFailedEvent), eventToDispatch);
                return;
            }

            var applicationSavedEvent = new ApplicationSavedEvent(applicationToSave);
            await client.RaiseEventAsync(context.InstanceId, nameof(ApplicationSavedEvent), applicationSavedEvent);
        }

        private void LogError(ILogger log, string instanceId, string error)
        {
            log.LogError($"Saving application failed instanceId: {instanceId}, error: {error}");
        }
    }
}