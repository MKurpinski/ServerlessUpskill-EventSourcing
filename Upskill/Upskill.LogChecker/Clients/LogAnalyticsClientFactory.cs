using System;
using Microsoft.Extensions.Options;
using Upskill.LogChecker.Options;

namespace Upskill.LogChecker.Clients
{
    public class LogAnalyticsClientFactory : ILogAnalyticsClientFactory
    {
        private readonly Lazy<ILogAnalyticsClient> _lazyClient;

        public LogAnalyticsClientFactory(IOptions<LogAnalyticsOptions> optionsAccessor)
        {
            var options = optionsAccessor.Value;

            _lazyClient = new Lazy<ILogAnalyticsClient>(() =>
            {
                var client = RestEase.RestClient.For<ILogAnalyticsClient>(options.BaseUrl);
                client.ApiKey = options.ApiKey;
                client.ApplicationId = options.ApplicationId;

                return client;
            });
        }

        public ILogAnalyticsClient GetClient()
        {
            return _lazyClient.Value;
        }
    }
}