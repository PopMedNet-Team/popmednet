using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Data Model with Request Types
    /// </summary>
    [DataContract]
    public class DataModelWithRequestTypesDTO : DataModelDTO
    {
        /// <summary>
        /// Available Request Types
        /// </summary>
        [DataMember]
        public IEnumerable<RequestTypeDTO> RequestTypes { get; set; }
    }
}
