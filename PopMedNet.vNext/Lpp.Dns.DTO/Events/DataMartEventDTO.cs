using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// DataMart Events
    /// </summary>
    [DataContract]
    public class DataMartEventDTO : BaseEventPermissionDTO
    {
        /// <summary>
        /// The ID of the DataMart
        /// </summary>
        [DataMember]
        public Guid DataMartID { get; set; }
    }
}
