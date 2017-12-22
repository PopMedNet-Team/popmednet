using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.Portal
{
    public class NetworkTokenData
    {
        public Guid NetworkID { get; set; }
        public string X509PublicKey { get; set; }
        public Guid ResearchOrganizationID { get; set; }
        public string Url { get; set; }
        public Guid ProviderOrganizationID { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
