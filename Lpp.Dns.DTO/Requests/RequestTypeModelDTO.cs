using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Request Type Models
    /// </summary>
    [DataContract]
    public class RequestTypeModelDTO
    {
        /// <summary>
        /// The ID of the Request Type
        /// </summary>
        [DataMember]
        public Guid RequestTypeID { get; set; }
        /// <summary>
        /// The ID of the Model
        /// </summary>
        [DataMember]
        public Guid DataModelID { get; set; }

    }
}
