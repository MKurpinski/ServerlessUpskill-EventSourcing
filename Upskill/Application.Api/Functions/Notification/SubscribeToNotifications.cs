using System.Threading.Tasks;
using Application.Api.CustomHttpRequests;
using Application.PushNotifications.Commands;
using Application.PushNotifications.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Application.Api.Functions.Notification
{
    public class SubscribeToNotifications
    {
        private readonly ISubscriptionHandler _subscriptionHandler;

        public SubscribeToNotifications(ISubscriptionHandler subscriptionHandler)
        {
            _subscriptionHandler = subscriptionHandler;
        }

        [FunctionName(nameof(SubscribeToNotifications))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Post, Route = "notification")] NotificationDeviceRegistrationHttpRequest registrationRequest)
        {
            var createSubscriptionCommand = new CreateSubscriptionCommand(
                registrationRequest.Platform,
                registrationRequest.Handle,
                registrationRequest.Tags);

            var result = await _subscriptionHandler.CreateSubscription(createSubscriptionCommand);

            if (!result.Success)
            {
                return new BadRequestObjectResult(result.Errors);
            }

            return new OkObjectResult(result.Value);
        }
    }
}
