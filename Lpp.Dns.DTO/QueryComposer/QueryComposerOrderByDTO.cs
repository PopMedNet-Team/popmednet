using Lpp.Dns.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO.QueryComposer
{
    /// <summary>
    /// Query composer OrderBy
    /// </summary>
    [DataContract]
    public class QueryComposerOrderByDTO
    {
        /// <summary>
        /// Perform OrderBy Directions
        /// </summary>
        [DataMember]
        public OrderByDirections Direction { get; set; }
    }
}
