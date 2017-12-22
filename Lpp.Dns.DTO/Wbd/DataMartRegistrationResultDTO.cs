using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
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
