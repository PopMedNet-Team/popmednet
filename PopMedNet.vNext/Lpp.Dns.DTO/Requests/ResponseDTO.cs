using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Response
    /// </summary>
    [DataContract]
    public class ResponseDTO : EntityDtoWithID
    {
        /// <summary>
        /// Gets or Sets the ID of Request DataMart
        /// </summary>
        [DataMember]
        public Guid RequestDataMartID { get; set; }
        /// <summary>
        /// Gets or Sets the ID of Response Group
        /// </summary>
        [DataMember]
        public Guid? ResponseGroupID { get; set; }
        /// <summary>
        /// Gets or sets the ID of responder who responds to the request
        /// </summary>
        [DataMember]
        public Guid? RespondedByID { get; set; }
        /// <summary>
        /// Gets or sets the response time
        /// </summary>
        [DataMember]
        public DateTimeOffset? ResponseTime { get; set; }
        /// <summary>
        /// Gets or sets the count
        /// </summary>
        [DataMember]
        public int Count { get; set; }
        /// <summary>
        /// The date the response was submitted on
        /// </summary>
        [DataMember]
        public DateTimeOffset SubmittedOn { get; set; }
        /// <summary>
        /// The ID of user that submitted the response
        /// </summary>
        [DataMember]
        public Guid SubmittedByID { get; set; }
        /// <summary>
        /// Submit message
        /// </summary>
        [DataMember]
        public string SubmitMessage { get; set; }
        /// <summary>
        /// Response message
        /// </summary>
        [DataMember]
        public string ResponseMessage { get; set; }
    }
}
