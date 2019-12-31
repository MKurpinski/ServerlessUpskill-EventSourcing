using System.Threading.Tasks;
using Application.Api.Events.External.ApplicationChanged;
using Application.Api.Events.External.Category;
using Application.Commands.Commands;
using Application.PushNotifications.Senders;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventPublisher.Publishers;

namespace Application.Api.Functions.ApplicationProcess
{
    public class ApplicationProcessFinishedEventPublisher
    {
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

            var categoryUsedEvent = new CategoryUsedEvent(command.Category, $"{nameof(DataStorage.Models.Application)}_{command.Id}");
            await _eventPublisher.PublishEvent(categoryUsedEvent);

            var applicationAddedEvent = _mapper.Map<SaveApplicationCommand, ApplicationChangedEvent>(command);
            await _eventPublisher.PublishEvent(applicationAddedEvent);

            await _pushNotificationSender.SendNotification(applicationAddedEvent, "New candidate has registered!");
        }
    }
}