using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities
{
    public static class ObjectEx
    {
        /// <summary>
        /// Appends an HTML line wrapped in a paragraph tag
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="line"></param>
        public static void AppendHtmlLine(this StringBuilder sb, string line)
        {
            sb.AppendFormat("<p>{0}</p>", line.Replace(System.Environment.NewLine, "</br>"));
        }

        /// <summary>
        /// Returns the description attribute of an Enum as applicable instead of just the tostring version.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="useDescription"></param>
        /// <returns></returns>
        public static string ToString(this Enum obj, bool useDescription) {
            if (!useDescription)
                return obj.ToStringEx();

            var field = obj.GetType().GetField(obj.ToString());

            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Any())
            {
                var description = ((DescriptionAttribute)attributes[0]).Description;
                return description;
            }
            else
            {
                return obj.ToStringEx();
            }
        }

        /// <summary>
        /// Returns a bool based on the object. Returns false if null.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool ToBool(this object obj)
        {
            if (obj.IsNull())
                return false;

            return Convert.ToBoolean(obj);
        }

        /// <summary>
        /// Returns an int based on obj. If null returns default for int 32
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Int32 ToInt32(this object obj)
        {
            if (obj.IsNull())
                return default(Int32);

            return Convert.ToInt32(obj);
        }

        [DebuggerStepThrough]
        public static Int32? ToNullableInt32(this object obj)
        {
            if (obj.IsNull())
                return null;

            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// Returns an int based on obj. If null returns default for int 64
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Int64 ToInt64(this object obj)
        {
            if (obj.IsNull())
                return default(Int64);

            return Convert.ToInt64(obj);
        }

        /// <summary>
        /// Converts the object to a double. Returns the default value of a double if the object is null.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static double ToDouble(this object obj)
        {
            if (obj.IsNull())
                return default(double);

            return Convert.ToDouble(obj);
        }

        [DebuggerStepThrough]
        public static string ToStringEx(this object obj, bool Trim = false)
        {
            if (obj.IsNull())
                return string.Empty;

            var result = obj.ToString();

            if (Trim)
                return result.Trim();

            return result;
        }

        /// <summary>
        /// Trims a string to the max length specified if longer.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="maxLength">The max length of the string.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string MaxLength(this string value, int maxLength)
        {            
            return MaxLength(value, maxLength, string.Empty);
        }

        /// <summary>
        /// Trims a string to the max length specified if longer.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="maxLength">The max length of the string including the appendWith value.</param>
        /// <param name="appendWith">A string value to append to the truncated string.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string MaxLength(this string value, int maxLength, string appendWith)
        {
            if (value.IsNullOrEmpty())
                return string.Empty;

            string s = value.Trim();
            if (s.Length > maxLength)
            {
                if(string.IsNullOrWhiteSpace(appendWith))
                    return s.Substring(0, maxLength);

                return s.Substring(0, maxLength - appendWith.Length) + appendWith;
            }

            return s;

        }

        [DebuggerStepThrough]
        public static DateTime? ToNullableDateTime(this string value)
        {
            DateTime d;
            if (DateTime.TryParse(value, out d))
            {
                return new Nullable<DateTime>(d);
            }
            return new Nullable<DateTime>();
        }

        [DebuggerStepThrough]
        public static bool AttributeExists<T>(this object obj) where T : class
        {
            Type attributeType = typeof(T);
            return obj.GetType().GetCustomAttributes(true).Any(x => x.GetType() == attributeType);
        }

        /// <summary>
        /// Returns a filtered list of assemblies in the current domain that are not system or obvious dependencies.
        /// </summary>
        /// <returns>A list of assemblies.</returns>
        [DebuggerStepThrough]
        public static IEnumerable<System.Reflection.Assembly> GetNonSystemAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.GetName().FullName.StartsWith("System.") &&
                                                                                    !a.GetName().FullName.StartsWith("Antlr3.") &&
                                                                                    !a.GetName().FullName.StartsWith("BouncyCastle.") &&
                                                                                    !a.GetName().FullName.StartsWith("CodeFirstStoreFunctions.") &&
                                                                                    !a.GetName().FullName.StartsWith("Common.Logging.") &&
                                                                                    !a.GetName().FullName.StartsWith("DocumentFormat.OpenXml.") &&
                                                                                    !a.GetName().FullName.StartsWith("EntityFramework.") &&
                                                                                    !a.GetName().FullName.StartsWith("FluentValidationNA.") &&
                                                                                    !a.GetName().FullName.StartsWith("Hangfire.") &&
                                                                                    !a.GetName().FullName.StartsWith("ICSharpCode.") &&
                                                                                    !a.GetName().FullName.StartsWith("LinqKit.") &&
                                                                                    !a.GetName().FullName.StartsWith("log4net.") &&
                                                                                    !a.GetName().FullName.StartsWith("Microsoft.") &&
                                                                                    !a.GetName().FullName.StartsWith("MigrationHelpers.") &&
                                                                                    !a.GetName().FullName.StartsWith("NCrontab.") &&
                                                                                    !a.GetName().FullName.StartsWith("Newtonsoft.") &&
                                                                                    !a.GetName().FullName.StartsWith("Owin.") &&
                                                                                    !a.GetName().FullName.StartsWith("Renci.") &&
                                                                                    !a.GetName().FullName.StartsWith("Theme.") &&
                                                                                    !a.GetName().FullName.StartsWith("WebGrease.")
                                                                                  );
        }
        
    }
}
