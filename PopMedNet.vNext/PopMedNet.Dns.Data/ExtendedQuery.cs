#nullable disable
using System.Linq.Expressions;

namespace PopMedNet.Dns.Data
{
    public class ExtendedQuery
    {
        public virtual Expression<Func<AclGlobal, bool>> Global { get; set; }

        public virtual Expression<Func<AclDataMart, bool>> DataMarts { get; set; }

        public virtual Expression<Func<AclGroup, bool>> Groups { get; set; }
        public virtual Expression<Func<AclOrganizationDataMart, bool>> OrganizationDataMarts { get; set; }
        public virtual Expression<Func<AclOrganization, bool>> Organizations { get; set; }
        public virtual Expression<Func<AclOrganizationUser, bool>> OrganizationUsers { get; set; }
        public virtual Expression<Func<AclProjectDataMart, bool>> ProjectDataMarts { get; set; }
        public virtual Expression<Func<AclProjectOrganization, bool>> ProjectOrganizations { get; set; }
        public virtual Expression<Func<AclProject, bool>> Projects { get; set; }
        public virtual Expression<Func<AclProjectRequestTypeWorkflowActivity, bool>> ProjectRequestTypeWorkflowActivity { get; set; }
        public virtual Expression<Func<AclRequest, bool>> Requests { get; set; }
        public virtual Expression<Func<AclUser, bool>> Users { get; set; }
        public virtual Expression<Func<AclRequestSharedFolder, bool>> SharedFolders { get; set; }
        public virtual Expression<Func<AclRegistry, bool>> Registries { get; set; }
        public virtual Expression<Func<AclTemplate, bool>> Templates { get; set; }
        public virtual Expression<Func<AclRequestType, bool>> RequestTypes { get; set; }

    }
}
#nullable enable
