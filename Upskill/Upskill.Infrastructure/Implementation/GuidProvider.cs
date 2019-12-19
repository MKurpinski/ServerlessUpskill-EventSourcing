using System;

namespace Upskill.Infrastructure.Implementation
{
    public class GuidProvider : IGuidProvider
    {
        public Guid GenerateGuid() => Guid.NewGuid();
    }
}
