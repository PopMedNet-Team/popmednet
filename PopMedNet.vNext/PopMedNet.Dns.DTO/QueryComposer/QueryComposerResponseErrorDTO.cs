﻿using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.QueryComposer
{
    /// <summary>
    /// Query composer Response Error
    /// </summary>
    [DataContract]
    public class QueryComposerResponseErrorDTO
    {
        /// <summary>
        /// Gets or sets the ID of the query that raised the error.
        /// </summary>
        [DataMember]
        public Guid? QueryID { get; set; }
        /// <summary>
        /// Gets or sets the code
        /// </summary>
        [DataMember]
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}