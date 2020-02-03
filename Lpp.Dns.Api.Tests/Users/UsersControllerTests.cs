using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Lpp.Dns.DTO;
using System.Linq;
using Lpp.Dns.Api.Users;
using System.Web.Http;
using Lpp.Dns.Data;
using Lpp.Utilities;

namespace Lpp.Dns.Api.Tests.Users
{
    [TestClass]
    public class UsersControllerTests : DataControllerTest<UsersController, UserDTO, User>
    {
        public UsersControllerTests() : base (new UserDTO {
            Active = true,
            ActivatedOn = DateTime.UtcNow,
            Deleted = false,
            Email = "test@unittest.com",
            FirstName = "Unit",
            LastName = "Test",
            ID = null,
            SignedUpOn = DateTime.UtcNow,
            Title = "Mr.",
            UserName = "unittest"
        }) { }

        [TestMethod]
        public async Task UsersByName() {
            var user = await controller.ByUserName("SystemAdministrator");
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public async Task UsersValidate()
        {
            var user = await controller.ValidateLogin(new LoginDTO
            {
                UserName = "SystemAdministrator",
                Password = "Password1!",
                RememberMe = false
            });

            Assert.IsNotNull(user);
        }

        [TestMethod]
        public async Task UsersForgotPassword()
        {
            await controller.ForgotPassword(new ForgotPasswordDTO
            {
                Email = "SystemAdministrator@root.org",
                UserName = "SystemAdministrator"
            });
        }

        [TestMethod]
        public void UsersLogout()
        {
            var result = controller.Logout();
            Assert.IsTrue(result.IsSuccessStatusCode);
        }


        [TestMethod]
        public async Task UsersUpdateLists()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;
            string username = "systemadministrator";
            string password = "Password1!";
            await controller.UpdateLookupLists(username, password);
        }

        #region Standard Tests

        [TestMethod]
        public async Task UsersGet() { await GetTest();}

        [TestMethod]
        public void UsersList() { ListTest(); }

        [TestMethod]
        public async Task UsersInsert() { await InsertTest(); }

        [TestMethod]
        public async Task UsersInsertOrDelete() { await InsertOrUpdateTest(); }

        [TestMethod]
        public async Task UsersUpdate() { await UpdateTest(); }

        [TestMethod]
        public async Task UsersDelete() { await DeleteTest(); }

        #endregion Standard Tests
    }
}
