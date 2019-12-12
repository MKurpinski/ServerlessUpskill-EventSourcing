using System.Linq;
using System.Threading.Tasks;
using Application.Api.Events.Internal;
using Application.Commands.Commands;
using Application.DataStorage.Model;
using Application.DataStorage.Repositories;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Application.Api.Functions.ApplicationProcess
{
    public class ApplicationSaver
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;

        public ApplicationSaver(
            IApplicationRepository applicationRepository,
            IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
        }

        [FunctionName(nameof(ApplicationSaver))]
        public async Task Run(
            [DurableClient] IDurableOrchestrationClient client,
            [ActivityTrigger] IDurableActivityContext context,
            ILogger log)
        {
            var command = context.GetInput<SaveApplicationCommand>();
            var applicationToSave = _mapper.Map<SaveApplicationCommand, DataStorage.Model.Application>(command);
            var saveResult = await _applicationRepository.Create(applicationToSave);

            if (!saveResult.Success)
            {
                this.LogError(log, context.InstanceId, string.Join(',', saveResult.Errors.Select(x => x.Value)));
                var eventToDispatch = new ApplicationSaveFailedInternalFunctionEvent(saveResult.Errors);
                await client.RaiseEventAsync(context.InstanceId, nameof(ApplicationSaveFailedInternalFunctionEvent), eventToDispatch);
                return;
            }

            var applicationSavedEvent = new ApplicationSavedInternalFunctionEvent(applicationToSave);
            await client.RaiseEventAsync(context.InstanceId, nameof(ApplicationSavedInternalFunctionEvent), applicationSavedEvent);
        }

        private void LogError(ILogger log, string instanceId, string error)
        {
            log.LogError($"Saving application failed instanceId: {instanceId}, error: {error}");
        }
    }
}