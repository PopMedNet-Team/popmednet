using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
{
    ///Types of Permission ACLs
    //Note: You must update the DataContext.HasGrantedPermissions any time you change this.
    [DataContract]
    public enum PermissionAclTypes
    {
        /// <summary>
        /// Global
        /// </summary>
        [EnumMember]
        Global = 0,
        /// <summary>
        /// DataMarts
        /// </summary>
        [EnumMember]
        DataMarts = 1,
        /// <summary>
        /// Groups
        /// </summary>
        [EnumMember]
        Groups = 2,
        /// <summary>
        /// Organizations
        /// </summary>
        [EnumMember]
        Organizations = 3,
        /// <summary>
        /// Projects
        /// </summary>
        [EnumMember]
        Projects = 4,
        /// <summary>
        /// Registries
        /// </summary>
        [EnumMember]
        Registries = 5,
        /// <summary>
        /// Requests
        /// </summary>
        [EnumMember]
        Requests = 6,
        /// <summary>
        /// Request Types
        /// </summary>
        [EnumMember]
        RequestTypes = 7,
        /// <summary>
        /// Request Shared Folders
        /// </summary>
        [EnumMember]
        RequestSharedFolders = 8,
        /// <summary>
        /// Users
        /// </summary>
        [EnumMember]
        Users = 9,
        /// <summary>
        /// Organization DataMarts
        /// </summary>
        [EnumMember]
        OrganizationDataMarts = 10,
        /// <summary>
        /// Project DataMarts
        /// </summary>
        [EnumMember]
        ProjectDataMarts = 11,
        /// <summary>
        /// Project DataMart Request Types
        /// </summary>
        [EnumMember]
        ProjectDataMartRequestTypes = 12,
        /// <summary>
        /// User Profile
        /// </summary>
        [EnumMember]
        UserProfile = 13,
        //[EnumMember]
        //ProjectUsers = 19,
        /// <summary>
        /// Project Organizations
        /// </summary>
        [EnumMember]
        ProjectOrganizations = 20,
        /// <summary>
        /// Organization Users
        /// </summary>
        [EnumMember]
        OrganizationUsers = 21,
        //[EnumMember]
        //ProjectOrganizationUsers = 22,
        /// <summary>
        /// Templates
        /// </summary>
        [EnumMember]
        Templates = 23,
        /// <summary>
        /// Project Request Type Workflow Activity
        /// </summary>
        [EnumMember]
        ProjectRequestTypeWorkflowActivity = 24
    }
}
