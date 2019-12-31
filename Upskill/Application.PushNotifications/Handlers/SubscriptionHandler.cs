using System;
using System.Linq;
using System.Threading.Tasks;
using Application.PushNotifications.Commands;
using Application.PushNotifications.Dtos;
using Application.PushNotifications.Factories;
using Application.PushNotifications.Providers;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using Upskill.Results;
using Upskill.Results.Implementation;

namespace Application.PushNotifications.Handlers
{
    public class SubscriptionHandler : ISubscriptionHandler
    {
        private readonly NotificationHubClient _hubClient;
        private readonly IRegistrationDescriptionFactory _registrationDescriptionFactory;
        private readonly ILogger<SubscriptionHandler> _logger;

        public SubscriptionHandler(
            IHubConnectionProvider provider,
            IRegistrationDescriptionFactory registrationDescriptionFactory,
            ILogger<SubscriptionHandler> logger)
        {
            _registrationDescriptionFactory = registrationDescriptionFactory;
            _logger = logger;
            _hubClient = provider.Get();
        }

        public async Task<IDataResult<RegistrationIdDto>> CreateSubscription(CreateSubscriptionCommand command)
        {
            try
            {
                var registrationId = await this.GetRegistrationId(command.Handle);

                var registrationDescriptionResult = _registrationDescriptionFactory.Build(
                    registrationId,
                    command.Platform,
                    command.Handle,
                    command.Tags);

                if (!registrationDescriptionResult.Success)
                {
                    return new FailedDataResult<RegistrationIdDto>(registrationDescriptionResult.Errors);
                }
                await _hubClient.CreateOrUpdateRegistrationAsync(registrationDescriptionResult.Value);
                return new SuccessfulDataResult<RegistrationIdDto>(new RegistrationIdDto(registrationId));
            }
            catch (Exception exception)
            {
                _logger.LogError($"{nameof(CreateSubscription)}: {exception}");
                return new FailedDataResult<RegistrationIdDto>();
            }
        }

        public async Task<IResult> DeleteSubscription(DeleteSubscriptionCommand command)
        {
            try
            {
                var registration = await _hubClient.GetRegistrationAsync<RegistrationDescription>(command.RegistrationId);
                if (registration == null)
                {
                    return new FailedResult();
                }

                await _hubClient.DeleteRegistrationAsync(command.RegistrationId);
                return new SuccessfulResult();
            }
            catch (Exception exception)
            {
                _logger.LogError($"{nameof(DeleteSubscription)}: {exception}");
                return new FailedResult();
            }
        }

        private async Task<string> GetRegistrationId(string handle)
        {
            const int singleResult = 1;
            var registrations = await _hubClient.GetRegistrationsByChannelAsync(handle, singleResult);

            if (registrations.Any())
            {
                return registrations.FirstOrDefault().RegistrationId;
            }

            return await _hubClient.CreateRegistrationIdAsync();
        }
    }
}