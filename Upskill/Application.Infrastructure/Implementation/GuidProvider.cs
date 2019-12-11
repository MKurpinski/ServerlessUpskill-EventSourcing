using System;

namespace Application.Infrastructure.Implementation
{
    public class GuidProvider : IGuidProvider
    {
        public Guid GenerateGuid() => Guid.NewGuid();
    }
}
