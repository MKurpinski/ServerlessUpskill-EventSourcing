using System;

namespace Upskill.Events.Mappers
{
    public interface ITypeMapper
    {
        string TypeName { get; }
        Type Type { get; }
    }
}
