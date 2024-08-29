using System;
using System.Collections.Generic;
using System.Text;

namespace PopMedNet.DMCS
{
    public static class ObjectExtensions
    {
        public static bool ByteEquals(this byte[] orig, byte[] compares)
        {
            if (orig == null && compares == null)
                return true;
            else if (orig == null || compares == null)
                return false;

            if (orig.Length != compares.Length)
                return false;

            for (int i = 0; i < orig.Length; i++)
            {
                if (orig[i] != compares[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the mime-type based on the extension of the file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMimeTypeByExtension(string fileName)
        {
            var extension = System.IO.Path.GetExtension(fileName).ToLower();
            switch (extension)
            {
                case "json":
                    return "application/json";
                case "xml":
                    return "application/xml";
                case "html":
                    return "text/html";
                case "txt":
                    return "text/plain";
                case "pdf":
                    return "application/pdf";
                case "doc":
                case "dot":
                    return "application/msword";
                case "docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case "dotx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                case "docm":
                    return "application/vnd.ms-word.document.macroEnabled.12";
                case "dotm":
                    return "application/vnd.ms-word.template.macroEnabled.12";
                case "xls":
                case "xlt":
                case "xla":
                    return "application/vnd.ms-excel";
                case "xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case "xltx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.template";
                case "xltm":
                    return "application/vnd.ms-excel.template.macroEnabled.12";
                case "xlsm":
                    return "application/vnd.ms-excel.sheet.macroEnabled.12";
                case "xlam":
                    return "application/vnd.ms-excel.addin.macroEnabled.12";
                case "xlsb":
                    return "application/vnd.ms-excel.sheet.binary.macroEnabled.12";
                case "ppt":
                case "pot":
                case "pps":
                case "ppa":
                    return "application/vnd.ms-powerpoint";
                case "pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case "potx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.template";
                case "ppsx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
                case "ppam":
                    return "application/vnd.ms-powerpoint.addin.macroEnabled.12";
                case "pptm":
                    return "application/vnd.ms-powerpoint.presentation.macroEnabled.12";
                case "potm":
                    return "application/vnd.ms-powerpoint.template.macroEnabled.12";
                case "ppsm":
                    return "application/vnd.ms-powerpoint.slideshow.macroEnabled.12";
                default:
                    string mimeType = "application/octet-stream";
                    //Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Path.GetExtension(fileName).ToLower());
                    //if (regKey != null && regKey.GetValue("Content Type") != null)
                    //    mimeType = regKey.GetValue("Content Type").ToString();
                    return mimeType;
            }
        }

        /// <summary>
        /// Makes sure the string is not null or DbNull.Value. If null an empty string is returned.
        /// </summary>
        /// <param name="value">The string value to ensure is not null.</param>
        /// <param name="trim">Trim the returned string value, default is false.</param>
        /// <returns></returns>
        public static string Ensure(this string value, bool trim = false)
        {
            if (string.IsNullOrEmpty(value) || DBNull.Value.Equals(value))
                return string.Empty;

            return trim ? value.Trim() : value;
        }
    }
}
