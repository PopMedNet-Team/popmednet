using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lpp.Dns.Data;
using System.Threading.Tasks;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.Security;

namespace Lpp.Dns.Api.Tests.Security
{
    [TestClass]
    public class Permissions
    {
        [TestMethod]
        public async Task PermissionsHasGranted()
        {
            using (var db = new DataContext())
            {
                var results = await db.HasGrantedPermissions(
                    new ApiIdentity(new Guid("96DC0001-94F1-47CC-BFE6-A22201424AD0"),
                    "SystemAdministrator", "System Administrator"),

                    PermissionIdentifiers.DataMart.RunAuditReport,
                PermissionIdentifiers.Portal.RunNetworkActivityReport,
                PermissionIdentifiers.Portal.RunEventsReport,
                PermissionIdentifiers.Portal.ListDataMarts,
                PermissionIdentifiers.Portal.ListGroups,
                PermissionIdentifiers.Project.View,
                PermissionIdentifiers.Project.Edit,
                //PermissionIdentifiers.Project.ListRequests,
                PermissionIdentifiers.Group.ListProjects,
                PermissionIdentifiers.Portal.ListRegistries,
                PermissionIdentifiers.Portal.ListOrganizations,
                PermissionIdentifiers.Portal.ListUsers);
            }
        }
    }
}
