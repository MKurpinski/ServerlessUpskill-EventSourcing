using System.Collections.Generic;

namespace Upskill.LogChecker.Clients.Models
{
    public class LogAnalyticsTable
    {
        public string Name { get; set; }
        public List<LogAnalyticsColumn> Columns { get; set; }
        public List<List<string>> Rows { get; set; }
    }
}