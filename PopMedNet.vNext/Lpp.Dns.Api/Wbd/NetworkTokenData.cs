using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Api.Wbd
{
    /// <summary>
    /// Used for passing data for registrations
    /// </summary>
    public class NetworkTokenData
    {
        /// <summary>
        /// The network ID to identify the network
        /// </summary>
        public Guid NetworkID { get; set; }
        /// <summary>
        /// The public key of the Research Network host
        /// </summary>
        public string X509PublicKey { get; set; }
        /// <summary>
        /// The owning research organization
        /// </summary>
        public Guid ResearchOrganizationID { get; set; }
        /// <summary>
        /// The Url of the research network
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// The Provider's organization from the research network.
        /// </summary>
        public Guid ProviderOrganizationID { get; set; }
        /// <summary>
        /// The expiry date of the token.
        /// </summary>
        public DateTime ExpiresOn { get; set; }
    }
}