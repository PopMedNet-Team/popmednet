using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

/// Port of Java SassyReader
namespace Lpp.Dns.General.SassyReader
{
    public class IO
    {
        //private static readonly String CHARSET_NAME = "windows-1252";
        private static readonly Encoding CHARSET_NAME = Encoding.ASCII;

        private IO()
        {
            // prevent instantiation
        }

        /**
         * Converts an int-array to a byte-array. Makes it more convenient to use
         * int literals in code.
         * 
         * @param arr
         * @return
         */
        public static byte[] toBytes(params int[] arr)
        {
            if (arr == null)
            {
                return null;
            }
            byte[] result = new byte[arr.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)arr[i];
            }
            return result;
        }

        public static String readString(byte[] buffer, int off, int len)
        {
            byte[] subset = readBytes(buffer, off, len);

            String str = getString(subset, CHARSET_NAME);
            return str;
        }

        private static String getString(byte[] bytes, Encoding encoding)
        {
            StreamReader reader = new StreamReader(new MemoryStream(bytes), encoding);
            return reader.ReadToEnd();
        }

        public static byte readByte(byte[] buffer, int off)
        {
            return buffer[off];
        }

        public static int readInt(byte[] buffer, int off)
        {
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(buffer, off, 4);
            return BitConverter.ToInt32(buffer, off);
        }

        public static double readDouble(byte[] buffer, int off)
        {
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(buffer, off, 8);
            return BitConverter.ToDouble(buffer, off);
        }

        public static byte[] readBytes(byte[] data, int off, int len)
        {
            if (data.Length < off + len)
            {
                throw new Exception("readBytes failed! data.length: " + data.Length + ", off: " + off + ", len: " + len);
            }
            byte[] subset = new byte[len];
            Array.Copy(data, off, subset, 0, len);
            return subset;
        }

        public static short readShort(byte[] buffer, int off)
        {
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(buffer, off, 2);
            return BitConverter.ToInt16(buffer, off);
        }

        public static Object readNumber(byte[] buffer, int off, int len)
        {
            if (len == 1)
            {
                return readByte(buffer, off);
            }
            else if (len == 2)
            {
                return readShort(buffer, off);
            }
            else if (len == 4)
            {
                return readInt(buffer, off);
            }
            else if (len == 8)
            {
                return readDouble(buffer, off);
            }
            else
            {
                throw new Exception("Number byte-length not supported: " + len);
            }
        }

        public static byte[] concat(byte[] arr1, byte[] arr2)
        {
            byte[] result = new byte[arr1.Length + arr2.Length];
            Array.Copy(arr1, 0, result, 0, arr1.Length);
            Array.Copy(arr2, 0, result, arr1.Length, arr2.Length);
            return result;
        }
    }
}
