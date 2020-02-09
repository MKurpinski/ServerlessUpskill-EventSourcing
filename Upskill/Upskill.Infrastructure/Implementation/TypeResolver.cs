using System;
using System.Collections.Concurrent;

namespace Upskill.Infrastructure.Implementation
{
    public class TypeResolver : ITypeResolver
    {
        private static readonly ConcurrentDictionary<string, Type> _typesDictionary;

        static TypeResolver()
        {
            _typesDictionary = new ConcurrentDictionary<string, Type>();
        }

        public Type Get(string typeFullName)
        {
            if (_typesDictionary.TryGetValue(typeFullName, out var type))
            {
                return type;
            }

            return _typesDictionary.GetOrAdd(typeFullName, Type.GetType(typeFullName));
        }
    }
}