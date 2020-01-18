using System.Threading.Tasks;
using Application.Category.Events.Outcoming;
using Application.Commands.Commands;
using Application.Core.Events.CreateApplicationProcessStarted;
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
            var command = context.GetInput<CreateApplicationCommand>();

            var categoryUsedEvent = new CategoryUsedEvent(command.Category, $"{APPLICATION_NAME}_{command.Id}", command.Id);
            await _eventPublisher.PublishEvent(categoryUsedEvent);

            var applicationCreatedEvent = _mapper.Map<CreateApplicationCommand, CreateApplicationProcessStartedEvent>(command);
            await _pushNotificationSender.SendNotification(applicationCreatedEvent, "New candidate has registered!");
        }
    }
}