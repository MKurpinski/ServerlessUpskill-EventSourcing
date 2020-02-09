using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Upskill.FunctionUtils.Results;
using Upskill.Infrastructure;
using Upskill.Infrastructure.Enums;
using Upskill.Infrastructure.Extensions;
using Upskill.RealTimeNotifications.NotificationSubscriberBinding;
using Upskill.RealTimeNotifications.Subscribers;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Application.Api.Functions.RebuildReadModel
{
    public class RequestReadModelRebuild
    {
        private readonly ISubscriber _subscriber;
        private readonly IGuidProvider _guidProvider;

        public RequestReadModelRebuild(
            ISubscriber subscriber,
            IGuidProvider guidProvider)
        {
            _subscriber = subscriber;
            _guidProvider = guidProvider;
        }

        [FunctionName(nameof(RequestReadModelRebuild))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Post, Route = "application/admin")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient processStarter,
            [NotificationSubscriber] string subscriber,
            ILogger log)
        {
            var correlationId = _guidProvider.GenerateGuid();
            await _subscriber.Register(correlationId, subscriber);

            await processStarter.StartNewAsync(nameof(RebuildReadModelProcessOrchestrator), correlationId);

            log.LogProgress(OperationPhase.Started, "Started applications read model rebuild", correlationId);
            return new AcceptedWithCorrelationIdHeaderResult(correlationId);
        }
    }
}
