using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Upskill.Cache;
using Upskill.Infrastructure.Enums;
using Upskill.Infrastructure.Extensions;

namespace Upskill.RealTimeNotifications.Subscribers
{
    public class Subscriber : ISubscriber
    {
        private const int MAX_SUBSCRIPTION_LIFETIME_IN_DAYS = 7;
        private readonly ICacheService _cacheService;
        private readonly ILogger<Subscriber> _logger;

        public Subscriber(
            ICacheService cacheService,
            ILogger<Subscriber> logger)
        {
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task Register(string correlationId, string subscriber)
        {
            if (string.IsNullOrEmpty(subscriber))
            {
                return;
            }

            await _cacheService.Set(correlationId, subscriber, TimeSpan.FromDays(MAX_SUBSCRIPTION_LIFETIME_IN_DAYS));
            _logger.LogProgress(OperationPhase.InProgress, $"Subscriber {subscriber} registered", correlationId);
        }
    }
}