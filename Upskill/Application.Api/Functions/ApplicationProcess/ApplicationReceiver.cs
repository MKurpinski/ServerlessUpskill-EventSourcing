using System.Threading.Tasks;
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
using Upskill.FunctionUtils.Extensions;
using Upskill.FunctionUtils.Results;
using Upskill.Infrastructure;
using Upskill.Infrastructure.Enums;
using Upskill.Infrastructure.Extensions;
using Upskill.RealTimeNotifications.NotificationSubscriberBinding;
using Upskill.RealTimeNotifications.Subscribers;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Application.Api.Functions.ApplicationProcess
{
    public class ApplicationReceiver
    {
        private readonly IGuidProvider _guidProvider;
        private readonly IFromFormToApplicationAddDtoRequestMapper _fromFormToApplicationAddDtoRequestMapper;
        private readonly ICommandBuilder<RegisterApplicationDto, RegisterApplicationCommand> _commandBuilder;
        private readonly ISubscriber _subscriber;

        public ApplicationReceiver(
            IGuidProvider guidProvider, 
            IFromFormToApplicationAddDtoRequestMapper fromFormToApplicationAddDtoRequestMapper,
            ICommandBuilder<RegisterApplicationDto, RegisterApplicationCommand> commandBuilder,
            ISubscriber subscriber)
        {
            _guidProvider = guidProvider;
            _fromFormToApplicationAddDtoRequestMapper = fromFormToApplicationAddDtoRequestMapper;
            _commandBuilder = commandBuilder;
            _subscriber = subscriber;
        }

        [FunctionName(nameof(ApplicationReceiver))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Post, Route = "application")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient processStarter,
            [NotificationSubscriber] string subscriber,
            ExecutionContext executionContext,
            ILogger log)
        {
            var instanceId = _guidProvider.GenerateGuid();
            executionContext.CorrelateExecution(instanceId);

            var mappingResult = await _fromFormToApplicationAddDtoRequestMapper.MapRequest(req);

            log.LogProgress(OperationPhase.Started, "Application process started", instanceId);

            if (!mappingResult.Success)
            {
                log.LogInformation($"Invalid data provided to the application process with instanceId: {instanceId}");
                return new BadRequestObjectResult(mappingResult.Errors);
            }

            await _subscriber.Register(instanceId, subscriber);

            var command = await _commandBuilder.Build(mappingResult.Value);

            await processStarter.StartNewAsync(nameof(ApplicationProcessOrchestrator), instanceId, command);

            log.LogProgress(OperationPhase.InProgress, "Started processing of application process", instanceId);

            return new AcceptedWithCorrelationIdHeaderResult(instanceId);
        }
    }
}
