using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Common Response Detail
    /// </summary>
    [DataContract]
    public class CommonResponseDetailDTO
    {
        /// <summary>
        /// Available Request Datamarts
        /// </summary>
        [DataMember]
        public IEnumerable<RequestDataMartDTO> RequestDataMarts { get; set; }
        /// <summary>
        /// Responses
        /// </summary>
        [DataMember]
        public IEnumerable<ResponseDTO> Responses { get; set; }
        /// <summary>
        /// Available Documents
        /// </summary>
        [DataMember]
        public IEnumerable<ExtendedDocumentDTO> Documents { get; set; }
        /// <summary>
        /// Indicates if the requesting user can approve or reject the responses.
        /// </summary>
        [DataMember]
        public bool CanViewPendingApprovalResponses { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool ExportForFileDistribution { get; set; }
    }
}
