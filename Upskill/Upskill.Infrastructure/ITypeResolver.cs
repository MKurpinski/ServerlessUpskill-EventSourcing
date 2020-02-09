using System;
using System.Collections.Generic;

namespace Upskill.Infrastructure
{
    public interface ITypeResolver
    {
        Type Get(string typeFullName);
    }
}
