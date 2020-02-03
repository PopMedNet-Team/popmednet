using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace mpost.SilverlightMultiFileUpload.Utils
{
    public class CustomUri : Uri
    {

        public CustomUri(string uri)
            : base(GetAbsoluteUrl(uri))
        {

        }

        public static string GetAbsoluteUrl(string strRelativePath)
        {
            if (string.IsNullOrEmpty(strRelativePath))
                return strRelativePath;

            string strFullUrl;
            if (strRelativePath.StartsWith("http:", StringComparison.OrdinalIgnoreCase)
              || strRelativePath.StartsWith("https:", StringComparison.OrdinalIgnoreCase)
              || strRelativePath.StartsWith("file:", StringComparison.OrdinalIgnoreCase)
              )
            {
                //already absolute
                strFullUrl = strRelativePath;
            }
            else
            {
                //relative, need to convert to absolute
                strFullUrl = System.Windows.Application.Current.Host.Source.AbsoluteUri;
                if (strFullUrl.IndexOf("ClientBin") > 0)
                    strFullUrl = strFullUrl.Substring(0, strFullUrl.IndexOf("ClientBin")) + strRelativePath;
                else
                    strFullUrl = strFullUrl.Substring(0, strFullUrl.LastIndexOf("/") + 1) + strRelativePath;
            }

            return strFullUrl;
        }

    }
}