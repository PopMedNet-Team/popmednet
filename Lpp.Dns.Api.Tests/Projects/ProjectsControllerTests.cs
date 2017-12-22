using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lpp.Dns.Api.Projects;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using System.Threading.Tasks;
using Lpp.Utilities;

namespace Lpp.Dns.Api.Tests.Projects
{
    [TestClass]
    public class ProjectsControllerTests : DataControllerTest<ProjectsController, ProjectDTO, Project>
    {
        public ProjectsControllerTests()
            : base(new ProjectDTO
            {
                Acronym = "Test",
                Active = true,
                Deleted = false,
                Description = "Test Project",
                GroupID = null,
                ID = null,
                Name = "Test Project",
                StartDate = DateTime.Today
            }) { }

        [TestMethod]
        public async Task ProjectsCopy()
        {
            var projectID = await controller.Copy(new Guid("06C20001-1C79-4260-915E-A22201477C58"));
        }

        [TestMethod]
        public async Task ProjectsGet() { await GetTest(); }

        [TestMethod]
        public void ProjectsList() { ListTest(); }

        [TestMethod]
        public async Task ProjectsInsert() { await InsertTest(); }

        [TestMethod]
        public async Task ProjectsInsertOrDelete() { await InsertOrUpdateTest(); }

        [TestMethod]
        public async Task ProjectsUpdate() { await UpdateTest(); }

        [TestMethod]
        public async Task ProjectsDelete() { await DeleteTest(); }

    }
}
