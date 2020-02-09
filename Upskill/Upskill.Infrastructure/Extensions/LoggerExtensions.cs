using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Upskill.Infrastructure.Enums;

namespace Upskill.Infrastructure.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogFailedOperation(this ILogger logger, OperationPhase phase, string messsage, IEnumerable<KeyValuePair<string, string>> errors, string correlationId)
        {
            var description = $"{messsage}, Errors: {string.Join(",", errors.Select(err => $"[{err.Key}]: {err.Value}"))}";
            var status = phase.ToString();
            logger.LogError(
                "Phase: {status},  Description: {description}, CorrelationId: {correlationId}", 
                status,
                description,
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
                "Phase: {status}, Message: {message}, CorrelationId: {correlationId}", 
                status,
                message,
                correlationId);
        }
    }
}