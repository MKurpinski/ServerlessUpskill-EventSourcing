using System.Collections.Generic;

namespace Application.Commands.Commands
{
    public class TrackProcessStatusCommand : ICommand
    {
        public string CorrelationId { get; }
        public string Status { get; }
        public IEnumerable<KeyValuePair<string, string>> Errors { get; }

        public TrackProcessStatusCommand(
            string correlationId,
            string status,
            IEnumerable<KeyValuePair<string, string>> errors)
        {
            CorrelationId = correlationId;
            Status = status;
            Errors = errors;
        }
    }
}
