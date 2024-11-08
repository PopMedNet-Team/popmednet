﻿using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// DataMart Registration Result
    /// </summary>
    [DataContract]
    public class DataMartRegistrationResultDTO
    {
        /// <summary>
        /// List of Datamarts
        /// </summary>
        [DataMember]
        public List<DataMartDTO> DataMarts { get; set; }
        /// <summary>
        /// List of Datamart models
        /// </summary>
        [DataMember]
        public List<DataMartInstalledModelDTO> DataMartModels { get; set; }
        /// <summary>
        /// list of users
        /// </summary>
        [DataMember]
        public List<UserWithSecurityDetailsDTO> Users { get; set; }
        /// <summary>
        /// Research organization
        /// </summary>
        [DataMember]
        public OrganizationDTO ResearchOrganization { get; set; }
        /// <summary>
        /// Provider Organization
        /// </summary>
        [DataMember]
        public OrganizationDTO ProviderOrganization { get; set; }
    }
}
