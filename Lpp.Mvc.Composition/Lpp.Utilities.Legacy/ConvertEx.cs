using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lpp.Utilities.Legacy
{
    public static class ConvertEx
    {
        /// <summary>
        /// Returns an int from any value passed. If null value passed, returns 0
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int ToInt32(this object val)
        {
            if (val == null || val == DBNull.Value)
                return 0;

            try
            {
                return Convert.ToInt32(val, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return 0;
            }
        }

        public static T[] Combine<T>(T[] first, T[] second)
        {
            var ret = new T[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        public static byte[] ToArray(this Stream stream)
        {
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static DateTime? ToNullableDateTime(this string value)
        {
            DateTime d;
            if (DateTime.TryParse(value, out d))
            {
                return new Nullable<DateTime>(d);
            }
            return new Nullable<DateTime>();
        }
    }
}
