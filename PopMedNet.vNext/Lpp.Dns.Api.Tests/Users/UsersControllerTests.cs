using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Lpp.Dns.DTO;
using System.Linq;
using Lpp.Dns.Api.Users;
using System.Web.Http;
using Lpp.Dns.Data;
using Lpp.Utilities;
using System.Configuration;
using Lpp.Dns.Data.Query;
using System.Web;
using Lpp.Utilities.Security;
using System.Security.Principal;

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
        public void UsersPasswordChangeTest()
        {
            var updateInfo = new UpdateUserPasswordDTO { Password = "Password1!", UserID = new Guid("e4eec242-58fb-4952-a78d-a65501281276") };
            using (var db = new DataContext())
            {
                //Update the password
                var user = db.Users.Find(updateInfo.UserID);
                string newHash = updateInfo.Password.ComputeHash();
                DateTimeOffset dateBack = DateTimeOffset.UtcNow.AddDays(ConfigurationManager.AppSettings["PreviousDaysPasswordRestriction"].ToInt32() * -1);
                int previousUses = ConfigurationManager.AppSettings["PreviousPasswordUses"].ToInt32();

                var param = new LastUsedPasswordCheckParams { User = user, DateRange = dateBack, NumberOfEntries = previousUses, Hash = newHash };

                var query = new LastUsedPasswordCheckQuery(db);

                if(user.PasswordHash == newHash || query.Execute(param))
                {
                    Assert.Fail("The password has already been used previously");
                }
            }
        }

        [TestMethod]
        public void CreateDummyUsers()
        {
            var ident = new ApiIdentity(new Guid("2CBF97E0-FF50-496A-8F77-A57DA62DAC05"), "SystemAdministrator", "System Administrator");
            HttpContext.Current.User = new GenericPrincipal(ident, new string[] { });

            var orgID = new Guid("7C5B0001-7635-4AC4-8961-A2F9013FFC50");
            var secGroupID = new Guid("7F180001-5B09-4EF5-8872-A2F9013FFC69");
            using (var db = new DataContext())
            {
                for (int i = 0; i < 1000; i++)
                {
                    var newUser = new User
                    {
                        FirstName = "Dummy",
                        LastName = "User",
                        UserName = $"DummyUser{i}",
                        Email = $"DummyUser{i}@test.com",
                        PasswordHash = "SomeBadPassword123!$".ComputeHash(),
                        Active = true,
                        FailedLoginCount = 0,
                        OrganizationID = orgID,
                    };

                    db.Users.Add(newUser);
                    db.SecurityGroupUsers.Add(new SecurityGroupUser { SecurityGroupID = secGroupID, UserID = newUser.ID });
                    db.SaveChanges();
                }                
            }
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
