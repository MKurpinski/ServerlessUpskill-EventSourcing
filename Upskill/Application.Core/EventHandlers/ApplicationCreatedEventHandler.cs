using System.Threading.Tasks;
using Application.Category.Events.Outcoming;
using Application.Core.Events;
using Application.PushNotifications.Senders;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;

namespace Application.Core.EventHandlers
{
    public class ApplicationCreatedEventHandler : IEventHandler<ApplicationCreatedEvent>
    {
        private const string APPLICATION_NAME = "Application";

        private readonly IEventPublisher _eventPublisher;
        private readonly IPushNotificationSender _pushNotificationSender;

        public ApplicationCreatedEventHandler(
            IEventPublisher eventPublisher,
            IPushNotificationSender pushNotificationSender)
        {
            _eventPublisher = eventPublisher;
            _pushNotificationSender = pushNotificationSender;
        }

        public async Task Handle(ApplicationCreatedEvent applicationCreatedEvent)
        {
            var categoryUsedEvent = new CategoryUsedEvent(applicationCreatedEvent.Category, $"{APPLICATION_NAME}_{applicationCreatedEvent.Id}", applicationCreatedEvent.Id);
            await _eventPublisher.PublishEvent(categoryUsedEvent);

            await _pushNotificationSender.SendNotification(applicationCreatedEvent, "New candidate has registered!");
        }
    }
}
