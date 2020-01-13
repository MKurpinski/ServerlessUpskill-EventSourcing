using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Upskill.Infrastructure.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogErrors(this ILogger logger, string message, IEnumerable<KeyValuePair<string, string>> errors)
        {
            logger.LogError($"{message}, errors: {string.Join(",", errors.Select(err => $"[{err.Key}]: {err.Value}"))}");
        }
    }
}