using Lpp.Dns.Data;
using Lpp.Utilities;
using Lpp.Utilities.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace Lpp.Dns.Api.Tests.DataMartClient
{
    [TestClass]
    public class DataMartClientTests
    {
        [TestMethod]
        public void GetDataMarts()
        {
            using (var controller = new TestDMCController())
            {
                var datamarts = controller.GetDataMarts().ToArray();
                Assert.IsTrue(datamarts.Length > 0);

                foreach (var dm in datamarts)
                {
                    Console.WriteLine("{2}/{1} (ID: {0:D})", dm.ID, dm.Name, dm.OrganizationName);
                }
            }
        }

        [TestMethod]
        public void GetRequestList()
        {
            using (var controller = new TestDMCController())
            {
                var criteria = new Lpp.Dns.DTO.DataMartClient.Criteria.RequestListCriteria { 
                    FromDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(30)).Date,
                    ToDate = DateTime.UtcNow
                };
                var list = AsyncHelpers.RunSync<Lpp.Dns.DTO.DataMartClient.RequestList>(() => controller.GetRequestList(criteria));
                Assert.IsNotNull(list);

                foreach (var row in list.Segment)
                {
                    Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, Allow Unattended Processing: {8},", row.DataMartName, row.Identifier, row.ModelName, row.Name, row.RequestTypePackageIdentifier, row.AdapterPackageVersion, row.RoutingStatus, row.Status, row.AllowUnattendedProcessing);
                }
            }
        }

        [TestMethod]
        public void GetRequestList2()
        {
            using (var controller = new TestDMCController())
            {
                var criteria = new Lpp.Dns.DTO.DataMartClient.Criteria.RequestListCriteria
                {
                    FromDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(30)).Date,
                    ToDate = DateTime.UtcNow,
                    FilterByDataMartIDs = new List<Guid> { new Guid("{807c0944-6884-4e9a-b425-f04958263d43}") },
                    //FilterByStatus = new[] { Lpp.Dns.DTO.DataMartClient.Enums.RequestStatuses.Submitted }
                };
                var list = AsyncHelpers.RunSync<Lpp.Dns.DTO.DataMartClient.RequestList>(() => controller.GetRequestList(criteria));
                Assert.IsNotNull(list);

                foreach (var row in list.Segment)
                {
                    Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, Allow Unattended Processing: {8},", row.DataMartName, row.Identifier, row.ModelName, row.Name, row.RequestTypePackageIdentifier, row.AdapterPackageVersion, row.RoutingStatus, row.Status, row.AllowUnattendedProcessing);
                }
            }
        }

        [TestMethod]
        public void GetRejectedRequestList()
        {
            using (var controller = new TestDMCController())
            {
                var criteria = new Lpp.Dns.DTO.DataMartClient.Criteria.RequestListCriteria
                {
                    FromDate = new DateTime(2000,1,1),
                    ToDate = DateTime.UtcNow,
                    FilterByStatus = new List<Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus> { Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.RequestRejected }

                };
                var list = AsyncHelpers.RunSync<Lpp.Dns.DTO.DataMartClient.RequestList>(() => controller.GetRequestList(criteria));
                Assert.IsNotNull(list);

                foreach (var row in list.Segment)
                {
                    Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, Allow Unattended Processing: {8},", row.DataMartName, row.Identifier, row.ModelName, row.Name, row.RequestTypePackageIdentifier, row.AdapterPackageVersion, row.RoutingStatus, row.Status, row.AllowUnattendedProcessing);
                }
            }
        }

        [TestMethod]
        public void GetRequestDetails()
        {
            using(var controller = new TestDMCController()){
                var criteria = new Lpp.Dns.DTO.DataMartClient.Criteria.RequestCriteria { ID = new[] { new Guid("{c7680001-ae10-4cfc-a818-a323016feaa6}") } };
                var list = controller.GetRequests(criteria);

                Assert.IsNotNull(list);

            }
        }

        [TestMethod]
        public void GetDocumentChunk()
        {         
            Guid documentID;
            int length;
            using(var db = new DataContext()){
                var document = db.Documents.Where(d => d.ItemID == new Guid("{c7680001-ae10-4cfc-a818-a323016feaa6}") && d.Viewable == false).Select(d => new { d.ID, d.Length }).First();
                documentID = document.ID;
                length = Convert.ToInt32(document.Length);
            }

            using (var controller = new TestDMCController())
            {
                System.Net.Http.HttpResponseMessage response = AsyncHelpers.RunSync<System.Net.Http.HttpResponseMessage>(() => controller.GetDocumentChunk(documentID, 0, length));
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
                var bytes = AsyncHelpers.RunSync<byte[]>(() =>  response.Content.ReadAsByteArrayAsync());
                Assert.IsNotNull(bytes);
            }
        }

        [TestMethod]
        public void PostResponseDocuments()
        {
            DataContext db = new DataContext();
            Response response = null;
            try
            {
                RequestDataMart routing = db.RequestDataMarts.Include(dm => dm.Responses ).Where(dm => dm.Status == DTO.Enums.RoutingStatus.Submitted && dm.Responses.Count > 0).OrderByDescending(dm => dm.Responses.OrderByDescending(rsp => rsp.Count).FirstOrDefault().SubmittedOn).FirstOrDefault();
                response = routing.AddResponse(db.Users.Where(u => u.UserName == TestDMCController.TestUserName).Select(u => u.ID).Single());
                db.SaveChanges();

                Guid[] result = null;
                using (var controller = new TestDMCController())
                {
                    var postData = new Lpp.Dns.DTO.DataMartClient.Criteria.PostResponseDocumentsData
                    {
                        RequestID = routing.RequestID,
                        DataMartID = routing.DataMartID,
                        Documents = new[] { new DTO.DataMartClient.Document { Name = "test-document.txt", IsViewable = false, MimeType = "text/plain", Size = 0 } }
                    };
                    result = AsyncHelpers.RunSync<Guid[]>(() => controller.PostResponseDocuments(postData));
                }

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count() > 0);

                Guid documentID = result.ToArray().First();
                Document doc = db.Documents.FirstOrDefault(d => d.ID == documentID);
                Assert.IsNotNull(doc);

            }
            finally
            {
                if (response != null)
                {
                    db.Documents.RemoveRange(db.Documents.Where(d => d.ItemID == response.ID));
                    db.Responses.Remove(response);
                    db.SaveChanges();
                }
                db.Dispose();
            }
        }

        [TestMethod]
        public void PostResponseDocumentsWithContent()
        {
            const string string1 = "This is a test document.";
            const string string2 = " This is the second sentence.";

            byte[] string1Data = Encoding.Default.GetBytes(string1);
            byte[] string2Data = Encoding.Default.GetBytes(string2);

            DataContext db = new DataContext();
            Response response = null;
            try
            {
                RequestDataMart routing = db.RequestDataMarts.Include(dm => dm.Responses).Where(dm => dm.Status == DTO.Enums.RoutingStatus.Submitted && dm.Responses.Count > 0).OrderByDescending(dm => dm.Responses.OrderByDescending(rsp => rsp.Count).FirstOrDefault().SubmittedOn).FirstOrDefault();
                response = routing.AddResponse(db.Users.Where(u => u.UserName == TestDMCController.TestUserName).Select(u => u.ID).Single());
                response.SubmittedOn = DateTime.Now;
                db.SaveChanges();

                Guid[] result = null;
                Guid documentID;
                using (var controller = new TestDMCController())
                {
                    var postData = new Lpp.Dns.DTO.DataMartClient.Criteria.PostResponseDocumentsData{
                        RequestID = routing.RequestID,
                        DataMartID = routing.DataMartID,
                        Documents = new[] { new DTO.DataMartClient.Document { Name = "test-document.txt", IsViewable = false, MimeType = "text/plain", Size = 0 } }
                    };
                    result = AsyncHelpers.RunSync<Guid[]>(() => controller.PostResponseDocuments(postData));

                    documentID = result.ToArray()[0];
                    System.Net.Http.HttpResponseMessage postResponse = AsyncHelpers.RunSync<System.Net.Http.HttpResponseMessage>(() => controller.PostResponseDocumentChunk(documentID, string1Data));
                    Assert.IsTrue(postResponse.StatusCode == System.Net.HttpStatusCode.OK);
                    postResponse = AsyncHelpers.RunSync<System.Net.Http.HttpResponseMessage>(() => controller.PostResponseDocumentChunk(documentID, string2Data));
                    Assert.IsTrue(postResponse.StatusCode == System.Net.HttpStatusCode.OK);
                }

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count() > 0);
                
                Document doc = db.Documents.FirstOrDefault(d => d.ID == documentID);
                Assert.IsNotNull(doc);

                byte[] docData = doc.GetData(db);

                Assert.AreEqual(string1Data.Length + string2Data.Length, docData.Length);
                string savedString = Encoding.Default.GetString(docData);
                Console.WriteLine(savedString);
            }
            finally
            {
                if (response != null)
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM Documents WHERE ItemID = @p0", response.ID);
                    db.Database.ExecuteSqlCommand("DELETE FROM RequestDataMartResponses WHERE ID = @p0", response.ID);
                    db.SaveChanges();
                }
                db.Dispose();
            }
        }

        [TestMethod]
        public void TestGettingDocumentLength()
        {
            using(var db = new DataContext()){
                Guid documentID = db.Documents.Where(d => d.Length > 0).OrderByDescending(d => d.CreatedOn).Select(d => d.ID).First();

                using (var stream = new Lpp.Dns.Data.Documents.DocumentStream(db, documentID))
                {
                    long docLength = stream.Length;
                    Assert.IsTrue(docLength > 0);
                }
            }
        }
    }

    public class TestDMCController : Lpp.Dns.Api.DataMartClient.DMCController
    {
        public const string TestUserName = "SystemAdministrator";
        readonly Utilities.Security.ApiIdentity _apiIdentity;

        public TestDMCController()
        {
            var user = DataContext.Users.Select(u => new { u.ID, u.UserName, u.FirstName, u.LastName, u.OrganizationID }).Single(u => u.UserName == TestUserName);
            _apiIdentity = new ApiIdentity(user.ID, user.UserName, (user.FirstName + " " + user.LastName).Trim(), user.OrganizationID); 
            
        }

        protected override ApiIdentity GetCurrentIdentity()
        {
            return _apiIdentity;
        }

        public Guid CurrentIdentityID
        {
            get
            {
                return _apiIdentity.ID;
            }
        }
        
    }
}
