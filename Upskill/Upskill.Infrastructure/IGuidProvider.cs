using System;

namespace Upskill.Infrastructure
{
    public interface IGuidProvider
    {
        string GenerateGuid(string format = "N");
    }
}
