using System;
using Microsoft.Azure.WebJobs.Description;

namespace Upskill.RealTimeNotifications.NotificationSubscriberBinding
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    [Binding]
    public sealed class NotificationSubscriberAttribute : Attribute
    {
    }
}
