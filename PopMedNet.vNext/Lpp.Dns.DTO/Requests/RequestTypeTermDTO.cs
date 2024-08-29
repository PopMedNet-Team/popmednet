using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Request Type Term
    /// </summary>
    [DataContract]
    public class RequestTypeTermDTO : EntityDto
    {
        /// <summary>
        /// Gets or Sets the ID of request type
        /// </summary>
        [DataMember]
        public Guid RequestTypeID { get; set; }
        /// <summary>
        /// Gets or Sets the ID of term
        /// </summary>
        [DataMember]
        public Guid TermID { get; set; }
        /// <summary>
        /// Term
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string Term { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string Description { get; set; }
        /// <summary>
        /// OID
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string OID { get; set; }
        /// <summary>
        /// Reference URL
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string ReferenceUrl { get; set; }
    }
}
