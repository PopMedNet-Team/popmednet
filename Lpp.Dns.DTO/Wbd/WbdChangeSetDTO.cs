using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// WBD change set
    /// </summary>
    [DataContract]
    public class WbdChangeSetDTO
    {
        /// <summary>
        /// Available requests
        /// </summary>
        [DataMember]
        public IEnumerable<RequestDTO> Requests { get; set; }
        /// <summary>
        /// Available Projects
        /// </summary>
        [DataMember]
        public IEnumerable<ProjectDTO> Projects { get; set; }
        /// <summary>
        /// Available datamarts
        /// </summary>
        [DataMember]
        public IEnumerable<DataMartDTO> DataMarts { get; set; }
        /// <summary>
        /// Available DataMart models
        /// </summary>
        [DataMember]
        public IEnumerable<DataMartInstalledModelDTO> DataMartModels { get; set; }
        /// <summary>
        /// Available request datamarts
        /// </summary>
        [DataMember]
        public IEnumerable<RequestDataMartDTO> RequestDataMarts { get; set; }
        /// <summary>
        /// Available Datamarts in project
        /// </summary>
        [DataMember]
        public IEnumerable<ProjectDataMartDTO> ProjectDataMarts { get; set; }
        /// <summary>
        /// Available Organizations
        /// </summary>
        [DataMember]
        public IEnumerable<OrganizationDTO> Organizations { get; set; }
        /// <summary>
        /// Available Documents
        /// </summary>
        [DataMember]
        public IEnumerable<RequestDocumentDTO> Documents { get; set; }
        /// <summary>
        /// Available list of users
        /// </summary>
        [DataMember]
        public IEnumerable<UserWithSecurityDetailsDTO> Users { get; set; }
        /// <summary>
        /// Available responses
        /// </summary>
        [DataMember]
        public IEnumerable<ResponseDetailDTO> Responses { get; set; }
        /// <summary>
        /// Available securityGroups
        /// </summary>
        [DataMember]
        public IEnumerable<SecurityGroupWithUsersDTO> SecurityGroups { get; set; }
        /// <summary>
        /// Available list of Request response security ACLs
        /// </summary>
        [DataMember]
        public IEnumerable<SecurityTupleDTO> RequestResponseSecurityACLs { get; set; }
        /// <summary>
        /// list of Datamart Security ACL
        /// </summary>
        [DataMember]
        public IEnumerable<SecurityTupleDTO> DataMartSecurityACLs { get; set; }
        /// <summary>
        /// Manage WBD ACLs
        /// </summary>
        [DataMember]
        public IEnumerable<SecurityTupleDTO> ManageWbdACLs { get; set; }
    }
}
