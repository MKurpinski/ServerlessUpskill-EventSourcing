using System;
using Upskill.Events;

namespace Upskill.EventsInfrastructure.Mappers
{
    public class TypeMapper<T> : ITypeMapper where T : IEvent
    {
        private readonly Lazy<Type> _lazyTType;

        public TypeMapper()
        {
            _lazyTType = new Lazy<Type>(() => typeof(T));
        }

        public string TypeName => _lazyTType.Value.Name;
        public Type Type => _lazyTType.Value;
    }
}