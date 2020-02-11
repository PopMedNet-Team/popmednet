using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities
{
    public static class Validation
    {
        [DebuggerStepThrough]
        public static bool IsNull(this object obj)
        {
            return obj == null || obj == DBNull.Value;
        }

        [DebuggerStepThrough]
        public static bool IsEmpty(this object obj)
        {
            return string.IsNullOrEmpty(obj.ToStringEx());
        }

        [DebuggerStepThrough]
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        [DebuggerStepThrough]
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}
