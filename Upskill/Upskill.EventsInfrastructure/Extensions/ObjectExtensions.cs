using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Upskill.EventsInfrastructure.Extensions
{
    public static class ObjectExtensions
    {
        public static async Task InvokeAsync(this object item, string methodName, object param)
        {
            var method = item
                .GetType()
                .GetMethods()
                .SingleOrDefault(m => m.Name == methodName);

            if (method == null)
            {
                return;
            }

            await method.InvokeAsync(item, param);
        }

        public static async Task InvokeAsync(this MethodInfo method, object obj, params object[] parameters)
        {
            var task = (Task)method.Invoke(obj, parameters);
            await task.ConfigureAwait(false);
        }
    }
}
