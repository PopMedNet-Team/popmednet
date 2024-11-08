﻿using PopMedNet.Dns.DTO.Enums;
using PopMedNet.Objects;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Organization EHRs
    /// </summary>
    [DataContract]
    public class OrganizationEHRSDTO : EntityDtoWithID
    {
        /// <summary>
        /// Gets or set the ID of organization
        /// </summary>
        [DataMember]
        public Guid OrganizationID { get; set; }
        /// <summary>
        /// Gets or sets the EHR types
        /// </summary>
        [DataMember]
        public EHRSTypes Type { get; set; }
        /// <summary>
        /// Gets or sets the EHRs systems
        /// </summary>
        [DataMember]
        public EHRSSystems System { get; set; }
        /// <summary>
        /// Gets or set the other
        /// </summary>
        [DataMember]
        public string? Other { get; set; }
        /// <summary>
        /// Returns start year
        /// </summary>
        [DataMember]
        public int? StartYear { get; set; }
        /// <summary>
        /// Returns end year
        /// </summary>
        [DataMember]
        public int? EndYear { get; set; } 
    }
}
