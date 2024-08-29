using Serilog;
using System;

namespace PopMedNet.DMCS
{
    public static class SerilogExtensions
    {
        public static ILogger AddResponse(this ILogger logger, Guid? id)
        {
            if (id.HasValue)
            {
                return logger.ForContext("ResponseID", id);
            }

            return logger;
        }
    }
}
