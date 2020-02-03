using Lpp.Dns.ApiClient;
using Lpp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Lpp.Dns.WebSites
{
    public static class DnsApiWebsiteServiceClient
    {
        public static DnsClient GetClient(string host = null, string userName = null, string password = null)
        {
            if (userName == null)
            {
                userName = WebConfigurationManager.AppSettings["ApiUserName"];
                password = WebConfigurationManager.AppSettings["ApiPassword"].DecryptString();
            }

            if (host == null)
                host = WebConfigurationManager.AppSettings["ServiceUrl"];

            var client = new DnsClient(host, userName, password);

            return client;
        }
    }
}
