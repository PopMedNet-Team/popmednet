using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Update RequestType models DTO.
    /// </summary>
    [DataContract]
    public class UpdateRequestTypeModelsDTO
    {
        /// <summary>
        /// Gets or sets the RequestType ID.
        /// </summary>
        [DataMember]
        public Guid RequestTypeID { get; set; }
        /// <summary>
        /// Gets or sets the DataModel IDs to associate with the RequestType.
        /// </summary>
        [DataMember]
        public IEnumerable<Guid> DataModels { get; set; }
    }
}
