using System;
using System.Threading.Tasks;

namespace Upskill.Infrastructure
{
    public interface IInvokerProvider
    {
        Func<object, object, Task> GetAsyncInvoker(object handler, object parameter, string methodName);
        Action<object, object> GetInvoker(object handler, object parameter, string methodName);
    }
}