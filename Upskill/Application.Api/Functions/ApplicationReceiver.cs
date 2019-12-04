using System.Threading.Tasks;
using Application.Api.CommandBuilders;
using Application.Api.Commands.RegisterApplicationCommand;
using Application.Api.Dtos;
using Application.Api.RequestToDtoMappers;
using Application.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using HttpMethods = Application.Api.Constants.HttpMethods;

namespace Application.Api.Functions
{
    public class ApplicationReceiver
    {
        private readonly IGuidProvider _guidProvider;
        private readonly IFromFormToApplicationAddDtoRequestMapper _fromFormToApplicationAddDtoRequestMapper;
        private readonly ICommandBuilder<RegisterApplicationDto, RegisterApplicationCommand> _commandBuilder;

        public ApplicationReceiver(
            IGuidProvider guidProvider, 
            IFromFormToApplicationAddDtoRequestMapper fromFormToApplicationAddDtoRequestMapper,
            ICommandBuilder<RegisterApplicationDto, RegisterApplicationCommand> commandBuilder)
        {
            _guidProvider = guidProvider;
            _fromFormToApplicationAddDtoRequestMapper = fromFormToApplicationAddDtoRequestMapper;
            _commandBuilder = commandBuilder;
        }

        [FunctionName(nameof(ApplicationReceiver))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Post, Route = "application")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient processStarter,
            ILogger log)
        {
            var mappingResult = await _fromFormToApplicationAddDtoRequestMapper.Deserialize(req);
            
            if (!mappingResult.Success)
            {
                return new BadRequestObjectResult(mappingResult.Errors);
            }

            var command = await _commandBuilder.Build(mappingResult.Value);
            var instanceId = _guidProvider.GenerateGuid().ToString();
            await processStarter.StartNewAsync(nameof(ApplicationProcessOrchestrator), instanceId, command);

            log.LogInformation($"Started orchestration of application process with instanceId: {instanceId}");
            return new AcceptedResult();
        }
    }
}
