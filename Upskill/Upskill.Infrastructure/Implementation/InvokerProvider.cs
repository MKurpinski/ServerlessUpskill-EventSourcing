using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Upskill.Infrastructure.Implementation
{
    public class InvokerProvider : IInvokerProvider
    {
        private static readonly Lazy<Type> _lazyTypeOfObject;
        private static readonly ConcurrentDictionary<string, Func<object, object, Task>> _asyncMemoize;
        private static readonly ConcurrentDictionary<string, Action<object, object>> _memoize;

        static InvokerProvider()
        {
            _lazyTypeOfObject = new Lazy<Type>(() => typeof(object));
            _asyncMemoize = new ConcurrentDictionary<string, Func<object, object, Task>>();
            _memoize = new ConcurrentDictionary<string, Action<object, object>>();
        }

        public Func<object, object, Task> GetAsyncInvoker(object instance, object parameter, string methodName)
        {
            var instanceType = instance.GetType();
            var parameterType = parameter.GetType();

            var memoizeKey = this.ConstructMemoizeKey(methodName, instanceType, parameterType);
            return _asyncMemoize.GetOrAdd(memoizeKey, this.GenerateInvoker<Func<object, object, Task>>(
                instanceType,
                parameterType,
                methodName,
                (arg1, arg2) => Task.CompletedTask));
        }

        public Action<object, object> GetInvoker(object instance, object parameter, string methodName)
        {
            var instanceType = instance.GetType();
            var parameterType = parameter.GetType();

            var memoizeKey = this.ConstructMemoizeKey(methodName, instanceType, parameterType);
            return _memoize.GetOrAdd(memoizeKey, this.GenerateInvoker<Action<object, object>>(
                instanceType,
                parameterType,
                methodName, 
                (arg1, arg2) => {}));
        }

        private string ConstructMemoizeKey(string methodName, Type instanceType, Type parameterType)
        {
            var memoizeKey = $"{instanceType.FullName}_{parameterType.FullName}_{methodName}";
            return memoizeKey;
        }


        private T GenerateInvoker<T>(Type handlerType, Type parameterType, string methodName, T defaultInvoker)
        {
            var instanceParameter = Expression.Parameter(_lazyTypeOfObject.Value);
            var argumentParameter = Expression.Parameter(_lazyTypeOfObject.Value);

            var methodToInvoke = handlerType
                .GetMethods()
                .SingleOrDefault(m =>
                {
                    var parameters = m.GetParameters();
                    return
                        m.Name == methodName
                        && parameters.Length == 1
                        && parameters.Single().ParameterType == parameterType;
                });

            if (methodToInvoke == null)
            {
                return defaultInvoker;
            }

            var methodInvocation = Expression.Call(
                Expression.Convert(instanceParameter, handlerType),
                methodToInvoke,
                Expression.Convert(argumentParameter, parameterType));

            var lambda = Expression.Lambda<T>(
                methodInvocation,
                instanceParameter,
                argumentParameter);

            var func = lambda.Compile();

            return func;
        }
    }
}
