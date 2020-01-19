using System;
using System.Threading.Tasks;
using Upskill.Cache;

namespace Upskill.RealTimeNotifications.Subscribers
{
    public class Subscriber : ISubscriber
    {
        private const int MAX_SUBSCRIPTION_LIFETIME_IN_DAYS = 7;
        private readonly ICacheService _cacheService;

        public Subscriber(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task Register(string correlationId, string subscriber)
        {
            if (string.IsNullOrEmpty(subscriber))
            {
                return;
            }

            await _cacheService.Set(correlationId, subscriber, TimeSpan.FromDays(MAX_SUBSCRIPTION_LIFETIME_IN_DAYS));
        }
    }
}