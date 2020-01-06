using System;

namespace Upskill.EventsInfrastructure.Mappers
{
    public interface ITypeMapper
    {
        string TypeName { get; }
        Type Type { get; }
    }
}
