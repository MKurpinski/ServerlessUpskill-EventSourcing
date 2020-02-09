using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private static readonly Lazy<string> _lazyTemplate;

        static LastLogProvider()
        {
            _lazyTemplate = new Lazy<string>(() =>
                "traces " +
                $"| extend {nameof(LogDto.CorrelationId)}  = tostring(customDimensions.prop__correlationId) " +
                $"| extend {nameof(LogDto.Status)} = tostring(customDimensions.prop__status) " +
                $"| extend {nameof(LogDto.Description)} = tostring(customDimensions.prop__message) " +
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
            var query = string.Format(_lazyTemplate.Value, correlationId);
            var response = await _logAnalyticsClient.GetLogs(new LogAnalyticsRequest(query));

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
            var status = this.ExtractRawValue(lastLog, dictionary, nameof(LogDto.Status));
            var rawTimestamp = this.ExtractRawValue(lastLog, dictionary, nameof(LogDto.Timestamp));

            var timestamp = DateTime.Parse(rawTimestamp);

            return new LogDto(correlationId, status, description, timestamp);
        }

        private string ExtractRawValue(IReadOnlyList<string> lastLog, IReadOnlyDictionary<string, int> dictionary, string propertyName)
        {
            return lastLog[dictionary[propertyName]];
        }
    }
}