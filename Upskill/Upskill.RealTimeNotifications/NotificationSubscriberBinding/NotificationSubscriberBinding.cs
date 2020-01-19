using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;

namespace Upskill.RealTimeNotifications.NotificationSubscriberBinding
{
    public class NotificationSubscriberBinding : IBinding
    {
        private const string REQUEST_BINDING_KEY = "req";
        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            
            var request = context.BindingData.Values.FirstOrDefault(x => x is DefaultHttpRequest) as DefaultHttpRequest;
            return Task.FromResult<IValueProvider>(new NotificationSubscriberValueProvider(request));
        }

        public bool FromAttribute => true;

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) => null;

        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor();
    }
}
