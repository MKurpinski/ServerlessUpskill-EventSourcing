using System;
using System.Collections.Generic;
using System.Linq;
using Upskill.Events.Mappers;
using Upskill.Results;
using Upskill.Results.Implementation;

namespace Upskill.EventsInfrastructure.Providers
{
    public class EventTypeProvider : IEventTypeProvider
    {
        private readonly IEnumerable<ITypeMapper> _typeMappers;

        public EventTypeProvider(IEnumerable<ITypeMapper> typeMappers)
        {
            _typeMappers = typeMappers;
        }

        public IDataResult<Type> ResolveEventType(string type)
        {
            var resolvedMapper =
                _typeMappers.FirstOrDefault(x => x.TypeName.Equals(type, StringComparison.OrdinalIgnoreCase));

            if (resolvedMapper == null)
            {
                return new FailedDataResult<Type>();
            }

            return new SuccessfulDataResult<Type>(resolvedMapper.Type);
        }
    }
}