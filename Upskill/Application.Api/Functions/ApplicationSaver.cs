using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Api.Events.Internal;
using Application.Commands.Commands;
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
            try
            {
                var command = context.GetInput<SaveApplicationCommand>();

                var applicationToSave = new DataStorage.Model.Application(
                    context.InstanceId,
                    command.CreationTime,
                    command.PhotoId,
                    command.CvId,
                    command.Category,
                    command.FirstName,
                    command.LastName);

                var saveResult = await _applicationRepository.Create(applicationToSave);

                if (!saveResult.Success)
                {
                    this.LogError(log, context.InstanceId, string.Join(',', saveResult.Errors.Select(x => x.Value)));
                    var eventToDispatch = new ApplicationSaveFailed();
                    await client.RaiseEventAsync(context.InstanceId, nameof(ApplicationSaveFailed), eventToDispatch);
                    return;
                }

                var applicationSavedEvent = new ApplicationSavedEvent(applicationToSave);
                await client.RaiseEventAsync(context.InstanceId, nameof(ApplicationSavedEvent), applicationSavedEvent);
            }
            catch (Exception ex)
            {
                this.LogError(log, context.InstanceId, ex.Message);
                var eventToDispatch = new ApplicationSaveFailed();
                await client.RaiseEventAsync(context.InstanceId, nameof(ApplicationSaveFailed), eventToDispatch);
            }
        }

        private void LogError(ILogger log, string instanceId, string error)
        {
            log.LogError($"Saving application failed instanceId: {instanceId}, error: {error}");
        }
    }
}