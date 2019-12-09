using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Extensions
{
    public class AcceptedWithCorrelationIdHeaderResult : AcceptedResult
    {
        private const string CORRELATION_ID_HEADER_NAME = "X-Correlation-ID";
        private readonly string _correlationId;

        public AcceptedWithCorrelationIdHeaderResult(string correlationId)
        {
            _correlationId = correlationId;
        }

        public override void OnFormatting(ActionContext context)
        {
            base.OnFormatting(context);
            context.HttpContext.Response.Headers.Add(CORRELATION_ID_HEADER_NAME, _correlationId);
        }
    }
}
