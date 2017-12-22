using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.QueryComposer {
    /// <summary>
    /// Query composer Select DTO
    /// </summary>
    [DataContract]
    public class QueryComposerSelectDTO {
        /// <summary>
        /// Available Fields
        /// </summary>
        [DataMember]
        public IEnumerable<QueryComposerFieldDTO> Fields { get; set; }
    }
}