using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Project DataMart within Request Type
    /// </summary>
    [DataContract]
    public class ProjectDataMartWithRequestTypesDTO : ProjectDataMartDTO
    {
        /// <summary>
        /// Available Request types
        /// </summary>
        [DataMember]
        public IEnumerable<RequestTypeDTO> RequestTypes { get; set; }
    }
}
