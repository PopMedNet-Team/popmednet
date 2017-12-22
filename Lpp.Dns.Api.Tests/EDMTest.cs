using System;
using System.Linq;
using System.Data.Entity;
using Lpp.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using System.Threading.Tasks;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.Security;

namespace Lpp.Dns.Api.Tests
{
    [TestClass]
    public class EDMTest
    {
        private ApiIdentity identity = new Utilities.Security.ApiIdentity(new Guid("96dc0001-94f1-47cc-bfe6-a22201424ad0"),
                    "System Administrator",
                    "SystemAdministrator");
        [TestMethod]
        public void TestDataLoads()
        {
            using (var db = new DataContext())
            {
                //var requestType = db.RequestTypes.Map<RequestType, RequestTypeDTO>().FirstOrDefault();

                var query = db.Requests.FirstOrDefault();

                var qdm = db.RequestDataMarts.FirstOrDefault();

                var project = db.Projects.FirstOrDefault();

                var pdm = db.ProjectDataMarts.FirstOrDefault();
            }
        }

        [TestMethod]
        public async Task TestHasGrantedPermissions()
        {
            using (var db = new DataContext())
            {
                var permissions = await db.HasGrantedPermissions<Project>(identity, new Guid("06c20001-1c79-4260-915e-a22201477c58"), PermissionIdentifiers.Project.Copy);
            }
        }

        [TestMethod]
        public void EDMFilterWithPermissions()
        {
            using (var db = new DataContext())
            {
                var query = db.Secure<DataMart>(identity);
                System.Diagnostics.Debug.Assert(query.Any());
            }
        }

        [TestMethod]
        public void TestTVF()
        {
            using (var db = new DataContext())
            {
                var userID = new Guid("9AC863E5-CBFE-4809-B4CA-A39500F90A70");
                var raw = db.Database.SqlQuery<Request>("SELECT * FROM dbo.FilteredRequestList('9AC863E5-CBFE-4809-B4CA-A39500F90A70')");

                var query = db.FilteredRequestList(userID);
                var results = query.ToArray();               
            }
        }
    }
}
