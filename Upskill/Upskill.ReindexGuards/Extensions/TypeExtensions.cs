using System;
using System.Collections.Generic;
using System.Linq;

namespace Upskill.ReindexGuards.Extensions
{
    public static class TypeExtensions
    {
        public static IReadOnlyCollection<Type> GetImplementedInterfaces(this Type type)
        {
            return type.GetInterfaces().ToList();
        }
    }
}
