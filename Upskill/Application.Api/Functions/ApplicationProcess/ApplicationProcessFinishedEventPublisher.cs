using System.Threading.Tasks;
using Application.Category.Events.Outcoming;
using Application.Commands.Commands;
using Application.Core.Events.ApplicationAddedEvent;
using Application.PushNotifications.Senders;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventsInfrastructure.Publishers;

namespace Application.Api.Functions.ApplicationProcess
{
    public class ApplicationProcessFinishedEventPublisher
    {
        private const string APPLICATION_NAME = "Application";
        private readonly IEventPublisher _eventPublisher;
        private readonly IPushNotificationSender _pushNotificationSender;
        private readonly IMapper _mapper;

        public ApplicationProcessFinishedEventPublisher(
            IEventPublisher eventPublisher,
            IMapper mapper,
            IPushNotificationSender pushNotificationSender)
        {
            _eventPublisher = eventPublisher;
            _mapper = mapper;
            _pushNotificationSender = pushNotificationSender;
        }

        [FunctionName(nameof(ApplicationProcessFinishedEventPublisher))]
        public async Task Run(
            [ActivityTrigger] IDurableActivityContext context)
        {
            var command = context.GetInput<SaveApplicationCommand>();

            var categoryUsedEvent = new CategoryUsedEvent(command.Category, $"{APPLICATION_NAME}_{command.Id}", command.Id);
            await _eventPublisher.PublishEvent(categoryUsedEvent);

            var applicationAddedEvent = _mapper.Map<SaveApplicationCommand, ApplicationAddedEvent>(command);
            await _pushNotificationSender.SendNotification(applicationAddedEvent, "New candidate has registered!");
        }
    }
}