using System;

namespace Application.Infrastructure
{
    public interface IGuidProvider
    {
        Guid GenerateGuid();
    }
}
