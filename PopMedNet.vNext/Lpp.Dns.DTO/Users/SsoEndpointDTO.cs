using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// SSO Endpoint
    /// </summary>
    [DataContract]
    public class SsoEndpointDTO : EntityDtoWithID
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        [Required, MaxLength(150)]
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        [Required]
        public string Description { get; set; }
        /// <summary>
        /// Post Url
        /// </summary>
         [DataMember]
        [Required]
        public string PostUrl { get; set; }
        /// <summary>
        /// oAuth Key
        /// </summary>
         [DataMember]
        public string oAuthKey { get; set; }
        /// <summary>
        /// oAuth Hash
        /// </summary>
         [DataMember]
        public string oAuthHash { get; set; }
        /// <summary>
         /// Gets or sets the required password
        /// </summary>
        [DataMember]
        public bool RequirePassword { get; set; }
        ///<summary>
        /// Gets or sets the Group ID
        /// </summary>
        [DataMember]
        public Guid Group { get; set; }
        /// <summary>
        /// Gets or sets the Display Index
        /// </summary>
        [DataMember]
        public int DisplayIndex { get; set; }
        /// <summary>
        /// Gets or sets if the endpoint is enabled
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

    }
}
