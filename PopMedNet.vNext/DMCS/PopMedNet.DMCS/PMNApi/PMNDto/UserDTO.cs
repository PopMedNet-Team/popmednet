using System;
using System.Runtime.Serialization;

namespace PopMedNet.DMCS.PMNApi.DTO
{
    [DataContract]
    public class UserDTO
    {
        [DataMember]
        public Guid? ID { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public bool Deleted { get; set; }
        [DataMember]
        public Guid? OrganizationID { get; set; }
        [DataMember]
        public string Organization { get; set; }
        [DataMember]
        public string OrganizationRequested { get; set; }
        [DataMember]
        public Guid? RoleID { get; set; }
        [DataMember]
        public string RoleRequested { get; set; }
        [DataMember]
        public DateTimeOffset? SignedUpOn { get; set; }
        [DataMember]
        public DateTimeOffset? ActivatedOn { get; set; }
        [DataMember]
        public DateTimeOffset? DeactivatedOn { get; set; }
        [DataMember]
        public Guid? DeactivatedByID { get; set; }
        [DataMember]
        public virtual string DeactivatedBy { get; set; }
        [DataMember]
        public string DeactivationReason { get; set; }
        [DataMember]
        public string RejectReason { get; set; }
        [DataMember]
        public DateTimeOffset? RejectedOn { get; set; }
        [DataMember]
        public Guid? RejectedByID { get; set; }
        [DataMember]
        public string RejectedBy { get; set; }
    }
}
