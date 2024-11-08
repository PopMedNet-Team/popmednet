﻿using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    [DataContract]
    public class MetadataEditPermissionsSummaryDTO
    {
        /// <summary>
        /// Gets or sets if the User has granted permissions to be able to edit at least one request within the system.
        /// </summary>
        [DataMember]
        public bool CanEditRequestMetadata { get; set; }

        /// <summary>
        /// Gets or sets a collection of DataMart IDs the user is able to edit the metadata for at least one route within the system.
        /// </summary>
        [DataMember]
        public IEnumerable<Guid> EditableDataMarts { get; set; }
    }
}
