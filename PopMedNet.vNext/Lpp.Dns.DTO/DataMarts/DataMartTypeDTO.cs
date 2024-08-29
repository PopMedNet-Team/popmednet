using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// DataMart Types
    /// </summary>
    [DataContract]
    public class DataMartTypeDTO
    {
        /// <summary>
        /// ID of DataMart type
        /// </summary>
        [DataMember]
        public Guid ID { get; set; }
        /// <summary>
        /// Name 
        /// </summary>
        [DataMember]
        public string Name { get; set; }
       
    }
}
