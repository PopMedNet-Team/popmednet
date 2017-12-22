using System;
using Lpp.Dns.Data;
using Lpp.Security;

namespace Lpp.Dns.Portal
{
    public static class VirtualSecObjects
    {
        //TODO: remove with future changes, these only exist to get back to compilable state.
        public static readonly Lpp.Security.SecurityObjectKind TempOrgSecurityGroupKind = new Lpp.Security.SecurityObjectKind("Organization Security Group");
        public static readonly Lpp.Security.SecurityObjectKind TempProjectSecurityGroupKind = new Lpp.Security.SecurityObjectKind("Project Security Group");

        public static readonly ISecurityObject Portal = new AnonymousObject("Portal", "{BBBA0001-2BC2-4E12-A5B4-A22100FDBAFD}", SecTargetKinds.PortalObjectKind);
        public static readonly ISecurityObject AllDataMarts = new AnonymousObject("All DataMarts", "{7A0C0001-B9A3-4F4B-9855-A22100FE0BA4}", DataMart.ObjectKind);
        public static readonly ISecurityObject AllOrganizations = new AnonymousObject("All Organizations", "{F3AB0001-DEF9-43D1-B862-A22100FE1882}", Organization.ObjectKind);
        public static readonly ISecurityObject AllGroups = new AnonymousObject("All Groups", "{6C380001-FD30-4A47-BC64-A22100FE22EF}", Group.ObjectKind);
        public static readonly ISecurityObject AllProjects = new AnonymousObject("All Projects", "{6A690001-7579-4C74-ADE1-A2210107FA29}", Project.ObjectKind);
        public static readonly ISecurityObject AllUsers = new AnonymousObject("All Users", "{1D3A0001-4717-40A3-98A1-A22100FDE0ED}", User.ObjectKind);
        public static readonly ISecurityObject AllOrgSecGroups = new AnonymousObject("All Org Security Groups", "{FEC20001-C54F-4FA8-B26D-A221011EB2C9}", TempOrgSecurityGroupKind);
        public static readonly ISecurityObject AllProjSecGroups = new AnonymousObject("All Project Security Groups", "{6EF80001-9126-4605-AA4D-A221011EC9F2}", TempProjectSecurityGroupKind);
        public static readonly ISecurityObject AllRequestTypes = new AnonymousObject("All Request Types", "{213A0001-9C0F-45C9-978A-A221011EDAEE}", Dns.RequestTypeSecObjectKind);
        public static readonly ISecurityObject AllRequests = new AnonymousObject("All Requests", "{EC260001-2AD7-4EC9-B492-A221011E5AF8}", Request.ObjectKind);
        public static readonly ISecurityObject AllEvents = new AnonymousObject("All Events", "{F8DF0001-A1BC-42CD-B920-A221011EF9A3}", SecTargetKinds.AuditEventObjectKind);
        public static readonly ISecurityObject AllPersonalEvents = new AnonymousObject("All Personal Events", "{D5EF0001-4122-477E-9C55-A2210142C609}", SecTargetKinds.AuditEventObjectKind);
        public static readonly ISecurityObject AllSharedFolders = new AnonymousObject("All Shared Folders", "{CD540001-6060-4AD0-8698-A2210142ADDD}", RequestSharedFolder.ObjectKind);
        public static readonly ISecurityObject AllRegistries = new AnonymousObject("All Registries", "{29CF75D9-1525-48A8-971D-8F9C3B8DDBD1}", Registry.ObjectKind);
        
        class AnonymousObject : ISecurityObject
        {
            readonly string _name;

            public AnonymousObject( string name, string sid, SecurityObjectKind kind ) 
            { 
                _name = name; 
                ID = new Guid( sid ); 
                Kind = kind; 
            }
            public Guid ID { get; private set; }

            public SecurityObjectKind Kind { get; private set; }           
            
            public override string ToString() { return _name; }
        }
    }
}