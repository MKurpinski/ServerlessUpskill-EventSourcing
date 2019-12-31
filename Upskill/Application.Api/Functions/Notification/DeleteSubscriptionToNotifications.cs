using System.Threading.Tasks;
using Application.PushNotifications.Commands;
using Application.PushNotifications.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Application.Api.Functions.Notification
{
    public class DeleteSubscriptionToNotifications
    {
        private readonly ISubscriptionHandler _subscriptionHandler;

        public DeleteSubscriptionToNotifications(ISubscriptionHandler subscriptionHandler)
        {
            _subscriptionHandler = subscriptionHandler;
        }

        [FunctionName(nameof(DeleteSubscriptionToNotifications))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Delete, Route = "notification/{id}")] HttpRequest req,
            string id)
        {
            var deleteSubscriptionCommand = new DeleteSubscriptionCommand(id);

            var result = await _subscriptionHandler.DeleteSubscription(deleteSubscriptionCommand);

            if (!result.Success)
            {
                return new NotFoundResult();
            }

            return new NoContentResult();
        }
    }
}
