﻿using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Organization Update EHRses
    /// </summary>
    [DataContract]
    public class OrganizationUpdateEHRsesDTO
    {
        /// <summary>
        /// Gets or set the ID of organization
        /// </summary>
        [DataMember]
        public Guid OrganizationID { get; set; }
        /// <summary>
        /// Available EHRs
        /// </summary>
        [DataMember]
        public IEnumerable<OrganizationEHRSDTO> EHRS { get; set; }
    }
}
