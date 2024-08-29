using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lpp.Dns.Api.Users;
using Lpp.Dns.DTO;
using Lpp.Dns.Data;
using System.Threading.Tasks;

namespace Lpp.Dns.Api.Tests.Users
{
    [TestClass]
    public class SsoEndpointControllerTests : DataControllerTest<SsoEndpointsController, SsoEndpointDTO, SsoEndpoint>
    {
        public SsoEndpointControllerTests() : base(new SsoEndpointDTO {
            Description = "Unit Test",
            Name = "Unit Test",
            oAuthHash = "",
            oAuthKey = "",
            PostUrl = "http://www.tempuri.org"
        }) { }

        #region Standard Tests

        [TestMethod]
        public async Task SsoEndpointGet() { await GetTest(); }

        [TestMethod]
        public void SsoEndpointList() { ListTest(); }

        [TestMethod]
        public async Task SsoEndpointInsert() { await InsertTest(); }

        [TestMethod]
        public async Task SsoEndpointInsertOrDelete() { await InsertOrUpdateTest(); }

        [TestMethod]
        public async Task SsoEndpointUpdate() { await UpdateTest(); }

        [TestMethod]
        public async Task SsoEndpointDelete() { await DeleteTest(); }

        #endregion Standard Tests
    }
}
