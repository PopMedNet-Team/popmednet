using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Dns.Sso
{
    public static class DnsHelpers
    {
        public static string ResourceUrl(this WebViewPage page, string Url) {
            return string.Format("{0}{1}", System.Web.Configuration.WebConfigurationManager.AppSettings["ResourceUrl"], Url);
        }

        public static string PortalUrl(this WebViewPage page, string Url)
        {
            return string.Format("{0}{1}", System.Web.Configuration.WebConfigurationManager.AppSettings["PortalUrl"], Url);
        }
    }
}