using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Upskill.Events.Extensions
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

        public static void Invoke(this object item, string methodName, object param)
        {
            var method = item
                .GetType()
                .GetMethods()
                .Where(m =>
                {
                    var parameters = m.GetParameters();
                    return
                        m.Name == methodName
                        && parameters.Length == 1
                        && parameters.Single().ParameterType == param.GetType();
                })
                .SingleOrDefault();

            if (method == null)
            {
                return;
            }

            method.Invoke(item, new []{ param });
        }

        public static async Task InvokeAsync(this MethodInfo method, object obj, params object[] parameters)
        {
            var task = (Task)method.Invoke(obj, parameters);
            await task.ConfigureAwait(false);
        }
    }
}
