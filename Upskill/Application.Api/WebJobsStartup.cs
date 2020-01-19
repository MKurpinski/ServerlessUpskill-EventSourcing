using Application.Api;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Upskill.RealTimeNotifications.NotificationSubscriberBinding;

[assembly: WebJobsStartup(typeof(WebJobsStartup))]

namespace Application.Api
{
    public class WebJobsStartup : IWebJobsStartup
    {
        void IWebJobsStartup.Configure(IWebJobsBuilder builder)
        {
            builder.AddNotificationSubscriberBinding();
        }
    }
}
