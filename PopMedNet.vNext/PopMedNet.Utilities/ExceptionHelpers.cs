using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Utilities
{
    [CLSCompliant(true)]
    public static class ExceptionHelpers
    {
        public static string UnwindException(this Exception exception, bool showStackTrace = false)
        {
            if (exception.InnerException == null)
                return exception.ToString();

            StringBuilder sb = new StringBuilder();
            UnwindException(exception, sb, showStackTrace);
            return sb.ToString();
        }

        public static void UnwindException(this Exception exception, StringBuilder sb, bool showStackTrace = false)
        {
            sb.AppendLine(showStackTrace ? exception.ToString() : exception.Message);

            if (exception.InnerException != null)
                UnwindException(exception.InnerException, sb, showStackTrace);
        }
    }
}
