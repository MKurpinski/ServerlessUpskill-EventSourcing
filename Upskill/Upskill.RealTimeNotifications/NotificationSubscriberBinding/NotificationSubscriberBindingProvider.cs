using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;

namespace Upskill.RealTimeNotifications.NotificationSubscriberBinding
{
    public class NotificationSubscriberBindingProvider : IBindingProvider
    {
        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            IBinding binding = new NotificationSubscriberBinding();
            return Task.FromResult(binding);
        }
    }

}
