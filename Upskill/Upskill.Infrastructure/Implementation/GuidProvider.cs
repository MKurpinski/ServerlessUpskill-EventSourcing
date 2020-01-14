using System;

namespace Upskill.Infrastructure.Implementation
{
    public class GuidProvider : IGuidProvider
    {
        public string GenerateGuid(string format = "N") => Guid.NewGuid().ToString(format);
    }
}
