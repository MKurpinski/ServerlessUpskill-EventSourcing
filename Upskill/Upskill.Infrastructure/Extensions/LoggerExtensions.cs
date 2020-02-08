using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Upskill.Infrastructure.Enums;

namespace Upskill.Infrastructure.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogFailedOperation(this ILogger logger, OperationPhase phase, string message, IEnumerable<KeyValuePair<string, string>> errors, string correlationId)
        {
            var status = phase.ToString();
            logger.LogError(
                $"Phase: {phase}, Message: {message}, Errors: {string.Join(",", errors.Select(err => $"[{err.Key}]: {err.Value}"))}, " +
                $"CorrelationId: {correlationId}", 
                status,
                correlationId);
        }

        public static void LogErrors(this ILogger logger, string message, IEnumerable<KeyValuePair<string, string>> errors)
        {
            logger.LogError($"Operation failed with {message}, errors: {string.Join(",", errors.Select(err => $"[{err.Key}]: {err.Value}"))}");
        }

        public static void LogProgress(this ILogger logger, OperationPhase phase, string message, string correlationId)
        {
            var status = phase.ToString();

            logger.LogInformation(
                $"Phase: {phase}, Message: {message}, CorrelationId: {correlationId}", 
                status,
                correlationId);
        }
    }
}