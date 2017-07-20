using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Lib.Css
{
    /// <summary>
    /// Extension methods for strings
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// Trims whitespaces including non printing 
        /// whitespaces like carriage returns, line feeds,
        /// and form feeds
        /// </summary>
        /// <param name="str">The string to trim</param>
        /// <returns></returns>
        public static String TrimWhiteSpace(this String str)
        {
            if (str == null)
            {
                return null;
            }
            Char[] whiteSpace = { '\r', '\n', '\f', '\t', '\v' };
            return str.Trim(whiteSpace).Trim();
        }

        public static String FixLineBreakForWeb(this String str)
        {
            return str.Replace(Environment.NewLine, "<br/>");
        }
        public static String FixTabsForWeb(this String str)
        {
            return str.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
        }
        public static String FixSpaceForWeb(this String str)
        {
            return str.Replace(" ", "&nbsp;");
        }
    }
}

