﻿using PopMedNet.Objects;
using PopMedNet.Objects.ValidationAttributes;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Report Aggregation Level
    /// </summary>
    [DataContract]
    public class ReportAggregationLevelDTO : EntityDtoWithID
    {
        /// <summary>
        /// Gets or sets the Network ID
        /// </summary>
        [DataMember]
        public Guid NetworkID { get; set; }
        /// <summary>
        /// Gets or set the Name of the network
        /// </summary>
        [DataMember, MaxLength(80)]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the Deleted on date
        /// </summary>
        [DataMember]
        public DateTime? DeletedOn { get; set; }        
        

    }
}
