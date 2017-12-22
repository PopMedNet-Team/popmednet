using System;
using Lpp.Objects;
using Lpp.Utilities.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lpp.Dns.DTO;
using Lpp.Dns.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Web.Http;
using Lpp.Utilities;
using Lpp.Utilities.WebSites.Controllers;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Api.Tests
{
    [TestClass]
    public class DataControllerTest<C, D, E> : ControllerTest<C>
        where C : LppApiDataController<E, D, DataContext, PermissionDefinition>, new()
        where D : EntityDtoWithID, new()
        where E : EntityWithID, new()
    {
        private D TestDTO;

        public DataControllerTest(D testDTO)
            : base()
        {
            if (testDTO.ID.HasValue)
                throw new ArgumentOutOfRangeException("testDTO", "The Test DTO passed must not have an ID and must not be in the database.");

            this.TestDTO = testDTO;
        }


        protected async Task GetTest()
        {
            try
            {
                var user = await controller.Get(Guid.Empty);
                Assert.IsNotNull(user);
            }
            catch (HttpResponseException e)
            {
                Assert.IsTrue(e.Response.IsSuccessStatusCode);
            }
            catch (ArgumentNullException)
            {
                //It's OK, security isn't giving us permission to see it.
            }
        }

        protected void ListTest()
        {
            var users = controller.List();
            Assert.IsNotNull(users);
        }
        
        protected async Task InsertTest()
        {
            D result = null;
            try
            {
                result = (await controller.Insert(new D[] {this.TestDTO})).First();
                Assert.IsNotNull(result);
            } catch(Exception) {
                throw;
            }

            //Delete it
            try {
                await controller.Delete(new Guid[] {result.ID.Value});
            } catch(Exception e) {
                throw new AggregateException(new Exception("Delete Failed. There may be incorrect data in the database that will manually need to be removed. Key: " + result.ID), e);
            }
        }

        protected async Task UpdateTest()
        {
            D result = null;
            try
            {
                result = (await controller.Insert(new D[] {this.TestDTO})).First();
                Assert.IsNotNull(result);
            } catch(Exception) {
                throw;
            }

            //Make a change
            try
            {
                result = (await controller.Update(new D[] { result })).First();
            }
            catch (Exception)
            {
                throw;
            }
            finally {
                AsyncHelpers.RunSync(() => controller.Delete(new Guid[] { result.ID.Value }));
            }
        }

        protected async Task InsertOrUpdateTest()
        {

            //Do the insert case
            var dto = (await controller.InsertOrUpdate(new D[] { this.TestDTO })).First();

            try {
                //Do the update case
                dto = (await controller.InsertOrUpdate(new D[] { dto})).First();
            } finally {
                //Delete the inserted item
                AsyncHelpers.RunSync(() => controller.Delete(new Guid[] {dto.ID.Value}));
            }
        }

        protected async Task DeleteTest()
        {
            //Adds and deletes
            await InsertTest();
        }

    }
}
