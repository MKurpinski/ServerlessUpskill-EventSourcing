using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestEase;
using Upskill.Infrastructure.Enums;
using Upskill.LogChecker.Clients;
using Upskill.LogChecker.Clients.Models;
using Upskill.LogChecker.Dtos;
using Upskill.Results;
using Upskill.Results.Implementation;

namespace Upskill.LogChecker.Providers
{
    public class LastLogProvider : ILastLogProvider
    {
        private readonly ILogAnalyticsClient _logAnalyticsClient;

        private static readonly Lazy<string> _lazyProgressQueryTemplate;
        private static readonly Lazy<string> _lazyExceptionQueryTemplate;

        static LastLogProvider()
        {
            _lazyProgressQueryTemplate = new Lazy<string>(() =>
                "traces " +
                $"| extend {nameof(LogDto.CorrelationId)}  = tostring(customDimensions.prop__correlationId) " +
                $"| extend {nameof(LogDto.Status)} = tostring(customDimensions.prop__status) " +
                $"| extend {nameof(LogDto.Description)} = tostring(customDimensions.prop__message) " +
                $"| extend {nameof(LogDto.Timestamp)} = timestamp " +
                $"| where {nameof(LogDto.CorrelationId)} == " + "\"{0}\" " +
                $"| project {nameof(LogDto.CorrelationId)}, {nameof(LogDto.Timestamp)}, {nameof(LogDto.Status)}, {nameof(LogDto.Description)} " +
                $"| order by {nameof(LogDto.Timestamp)} desc " +
                "| limit 1");

            _lazyExceptionQueryTemplate = new Lazy<string>(() =>
                "exceptions " +
                $"| extend {nameof(LogDto.CorrelationId)}  = tostring(operation_ParentId) " +
                $"| extend {nameof(LogDto.Status)} = \"Failed\"" +
                $"| extend {nameof(LogDto.Description)} = tostring(problemId) " +
                $"| extend {nameof(LogDto.Timestamp)} = timestamp " +
                $"| where {nameof(LogDto.CorrelationId)} == " + "\"{0}\" " +
                $"| project {nameof(LogDto.CorrelationId)}, {nameof(LogDto.Timestamp)}, {nameof(LogDto.Status)}, {nameof(LogDto.Description)} " +
                $"| order by {nameof(LogDto.Timestamp)} desc " +
                "| limit 1");
        }

        public LastLogProvider(ILogAnalyticsClientFactory logAnalyticsClientFactory)
        {
            _logAnalyticsClient = logAnalyticsClientFactory.GetClient();
        }

        public async Task<IDataResult<LogDto>> GetLastLogByCorrelationId(string correlationId)
        {
            var query = string.Format(_lazyProgressQueryTemplate.Value, correlationId);
            var response = await _logAnalyticsClient.GetLogs(new LogAnalyticsRequest(query));

            var lastProgressLogResult = this.GetLogFromResponse(response);
            var isOperationFinished =
                lastProgressLogResult.Success && 
                (lastProgressLogResult.Value.Status == OperationStatus.Finished ||
                lastProgressLogResult.Value.Status == OperationStatus.Failed);

            if (isOperationFinished)
            {
                return new SuccessfulDataResult<LogDto>(lastProgressLogResult.Value);
            }

            query = string.Format(_lazyExceptionQueryTemplate.Value, correlationId);
            response = await _logAnalyticsClient.GetLogs(new LogAnalyticsRequest(query));
            var exceptionLogResult = this.GetLogFromResponse(response);

            if (exceptionLogResult.Success)
            {
                return new SuccessfulDataResult<LogDto>(exceptionLogResult.Value);
            }

            if (!lastProgressLogResult.Success)
            {
                return new FailedDataResult<LogDto>();
            }

            return new SuccessfulDataResult<LogDto>(lastProgressLogResult.Value);
        }

        private IDataResult<LogDto> GetLogFromResponse(Response<LogAnalyticsResponse> response)
        {
            if (!response.ResponseMessage.IsSuccessStatusCode)
            {
                return new FailedDataResult<LogDto>();
            }

            var content = response.GetContent();

            var table = content.Tables.FirstOrDefault();
            if (table == null)
            {
                return new FailedDataResult<LogDto>();
            }

            var columns = table.Columns;

            if (!columns.Any())
            {
                return new FailedDataResult<LogDto>();
            }

            var lastLog = table.Rows.FirstOrDefault();

            if (lastLog == null)
            {
                return new FailedDataResult<LogDto>();
            }

            var lastLogDto = this.FromRowToLogDto(columns, lastLog);
            return new SuccessfulDataResult<LogDto>(lastLogDto);
        }

        private LogDto FromRowToLogDto(IEnumerable<LogAnalyticsColumn> columns, IReadOnlyList<string> lastLog)
        {
            var dictionary = columns
                .Select((c, index) => new {Key = c.Name, Value = index})
                .ToDictionary(x => x.Key, x => x.Value);

            var description = this.ExtractRawValue(lastLog, dictionary, nameof(LogDto.Description));
            var correlationId = this.ExtractRawValue(lastLog, dictionary, nameof(LogDto.CorrelationId));
            var rawStatus = this.ExtractRawValue(lastLog, dictionary, nameof(LogDto.Status));
            var rawTimestamp = this.ExtractRawValue(lastLog, dictionary, nameof(LogDto.Timestamp));

            var timestamp = DateTime.Parse(rawTimestamp);
            var canParse = Enum.TryParse<OperationStatus>(rawStatus, out var status);
            if (!canParse)
            {
                status = OperationStatus.Unknown;
            }

            return new LogDto(correlationId, status, description, timestamp);
        }

        private string ExtractRawValue(IReadOnlyList<string> lastLog, IReadOnlyDictionary<string, int> dictionary, string propertyName)
        {
            return lastLog[dictionary[propertyName]];
        }
    }
}