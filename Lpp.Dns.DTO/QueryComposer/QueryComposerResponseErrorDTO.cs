using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO.QueryComposer
{
    /// <summary>
    /// Query composer Response Error
    /// </summary>
    [DataContract]
    public class QueryComposerResponseErrorDTO
    {
        /// <summary>
        /// Gets or sets the code
        /// </summary>
        [DataMember]
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
