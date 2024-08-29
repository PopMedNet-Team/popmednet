using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Request search Term
    /// </summary>
    [DataContract]
    public class RequestSearchTermDTO
    {
        /// <summary>
        /// Gets or sets the Type
        /// </summary>
        [DataMember]
        public int Type { get; set; }
        /// <summary>
        /// String value
        /// </summary>
        [DataMember]
        public string StringValue { get; set; }
        /// <summary>
        /// Gets or sets the value of request number
        /// </summary>
        [DataMember]
        public decimal? NumberValue { get; set; }
        /// <summary>
        /// Date from
        /// </summary>
        [DataMember]
        public DateTimeOffset? DateFrom { get; set; }
        /// <summary>
        /// Date To
        /// </summary>
        [DataMember]
        public DateTimeOffset? DateTo { get; set; }
        /// <summary>
        /// Numbe From
        /// </summary>
        [DataMember]
        public decimal? NumberFrom { get; set; }
        /// <summary>
        /// Gets or sets the Number To decimal
        /// </summary>
        [DataMember]
        public decimal? NumberTo { get; set; }
        /// <summary>
        /// Gets or sets the ID of request
        /// </summary>
        [DataMember]
        public Guid RequestID { get; set; }
    }
}
