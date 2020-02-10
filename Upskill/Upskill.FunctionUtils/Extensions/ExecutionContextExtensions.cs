using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Azure.WebJobs;

namespace Upskill.FunctionUtils.Extensions
{
    public static class ExecutionContextExtensions
    {
        public static void CorrelateExecution(this ExecutionContext context, string correlationId)
        {
            var activity = new Activity(context.FunctionName);
            activity.SetParentId(correlationId);
            activity.Start();
        }
    }
}
