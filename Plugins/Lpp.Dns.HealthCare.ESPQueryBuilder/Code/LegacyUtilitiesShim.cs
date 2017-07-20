using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Lpp.Dns.HealthCare.ESPQueryBuilder
{
    /// <summary>
    /// Utility class containing extensions that were part of Lpp.Utilities.Legacy and not part of Lpp.Utilities.
    /// </summary>
    public static class LegacyUtilitiesShim
    {
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> s)
        {
            return s == null || !s.Any();
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
    }
}
