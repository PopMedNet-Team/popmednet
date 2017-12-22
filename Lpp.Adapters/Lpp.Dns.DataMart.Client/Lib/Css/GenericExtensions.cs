using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Lib.Css
{
    /// <summary>
    /// Generic Extension methods
    /// </summary>
    public static partial class GenericExtensions
    {
        /// <summary>
        /// Returns the Default value if the current Object if it is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="DefaultValue">Default value for the Object if it is null. If it is a string it will check for null or empty or whitespace</param>
        /// <returns></returns>
        public static T Default<T>(this T value, T DefaultValue)
        {
            if (value == null)
            {
                return DefaultValue;
            }
            
            if (typeof(T) == typeof(String))
            {
                if (String.IsNullOrWhiteSpace((value as String)))
                {
                    return DefaultValue;
                }
            }
            return value;
        }

    }

}

