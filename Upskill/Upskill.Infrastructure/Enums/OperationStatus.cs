using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Upskill.Infrastructure.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OperationStatus
    {
        Started = 0,
        InProgress = 1,
        Finished = 2,
        Failed = 3,
        Unknown = 4
    }
}
