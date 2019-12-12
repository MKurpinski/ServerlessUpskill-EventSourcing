using System.Threading.Tasks;
using Application.Api.Results;
using Application.Commands.CommandBuilders;
using Application.Commands.Commands;
using Application.RequestMappers.Dtos;
using Application.RequestMappers.RequestToDtoMappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Upskill.Infrastructure;
using HttpMethods = Application.Api.Constants.HttpMethods;

namespace Application.Api.Functions.ApplicationProcess
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
            var mappingResult = await _fromFormToApplicationAddDtoRequestMapper.MapRequest(req);

            var instanceId = _guidProvider.GenerateGuid().ToString("N");

            if (!mappingResult.Success)
            {
                log.LogInformation($"Invalid data provided to the application process with instanceId: {instanceId}");
                return new BadRequestObjectResult(mappingResult.Errors);
            }

            var command = await _commandBuilder.Build(mappingResult.Value);

            await processStarter.StartNewAsync(nameof(ApplicationProcessOrchestrator), instanceId, command);

            log.LogInformation($"Started orchestration of application process with instanceId: {instanceId}");

            return new AcceptedWithCorrelationIdHeaderResult(instanceId);
        }
    }
}
