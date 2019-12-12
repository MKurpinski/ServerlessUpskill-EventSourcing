using System;

namespace Upskill.Infrastructure
{
    public interface IGuidProvider
    {
        Guid GenerateGuid();
    }
}
