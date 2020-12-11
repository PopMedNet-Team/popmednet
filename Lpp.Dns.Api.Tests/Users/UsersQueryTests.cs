using Lpp.Dns.Data;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lpp.Dns.Api.Tests.Users
{
    [TestClass]
    public class UsersQueryTests
    {
        static readonly log4net.ILog Logger;

        static UsersQueryTests()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = log4net.LogManager.GetLogger(typeof(UsersQueryTests));
        }

        readonly ApiIdentity _identity;

        public UsersQueryTests()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "https://tempuri.org", ""),
                new HttpResponse(new StringWriter())
            );

            //Set the current user
            _identity = new ApiIdentity(new Guid("2CBF97E0-FF50-496A-8F77-A57DA62DAC05"), "SystemAdministrator", "System Administrator");
            HttpContext.Current.User = new GenericPrincipal(_identity, new string[] { });
        }

        [TestMethod]
        public async Task GetMetadataEditPermissionsSummary_FromController()
        {
            using(var controller = new Lpp.Dns.Api.Users.UsersController())
            {
                controller.Request = new System.Net.Http.HttpRequestMessage();
                controller.RequestContext = new System.Web.Http.Controllers.HttpRequestContext();
                controller.RequestContext.Principal = HttpContext.Current.User;                

                var result = await controller.GetMetadataEditPermissionsSummary();

                Assert.IsTrue(result.CanEditRequestMetadata);
            }
        }

        [TestMethod]
        public async Task GetMetadataPermissionQuery()
        {
            using (var DataContext = new Data.DataContext())
            {
                DataContext.Database.Log = (sql) => {
                    Logger.Debug(sql);
                };

                var result = new DTO.MetadataEditPermissionsSummaryDTO { CanEditRequestMetadata = false, EditableDataMarts = Enumerable.Empty<Guid>() };
                var globalAcls = DataContext.GlobalAcls.FilterAcl(_identity, PermissionIdentifiers.Request.Edit);
                var projectAcls = DataContext.ProjectAcls.FilterAcl(_identity, PermissionIdentifiers.Request.Edit, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditRequestMetadata);
                var projectOrganizationAcls = DataContext.ProjectOrganizationAcls.FilterAcl(_identity, PermissionIdentifiers.Request.Edit);

                result.CanEditRequestMetadata = await (from r in DataContext.Secure<Request>(_identity).AsNoTracking()
                                                       let gAcl = globalAcls
                                                       let pAcl = projectAcls.Where(a => a.ProjectID == r.ProjectID)
                                                       let poAcl = projectOrganizationAcls.Where(a => a.ProjectID == r.ProjectID && a.OrganizationID == r.OrganizationID)
                                                       where ((gAcl.Any() || pAcl.Where(a => a.PermissionID == PermissionIdentifiers.Request.Edit.ID).Any() || poAcl.Any()) && (gAcl.All(a => a.Allowed) && pAcl.Where(a => a.PermissionID == PermissionIdentifiers.Request.Edit.ID).All(a => a.Allowed) && poAcl.All(a => a.Allowed))) &&
                                                              ((int)r.Status < 500 ? true : (pAcl.Where(a => a.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditRequestMetadata.ID).Any() && pAcl.Where(a => a.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditRequestMetadata.ID).All(a => a.Allowed)))
                                                       select r).AnyAsync();

                if (result.CanEditRequestMetadata)
                {

                    var datamarts = DataContext.Secure<DataMart>(_identity, PermissionIdentifiers.DataMartInProject.SeeRequests);
                    var requests = DataContext.Secure<Request>(_identity);

                    result.EditableDataMarts = await (from rdm in DataContext.RequestDataMarts
                                                      join dm in datamarts on rdm.DataMartID equals dm.ID
                                                      join r in requests on rdm.RequestID equals r.ID
                                                      let gAcl = globalAcls
                                                      let pAcl = projectAcls.Where(a => a.ProjectID == r.ProjectID)
                                                      let poAcl = projectOrganizationAcls.Where(a => a.ProjectID == r.ProjectID && a.OrganizationID == r.OrganizationID)
                                                      where ((gAcl.Any() || pAcl.Where(a => a.PermissionID == PermissionIdentifiers.Request.Edit.ID).Any() || poAcl.Any()) && (gAcl.All(a => a.Allowed) && pAcl.Where(a => a.PermissionID == PermissionIdentifiers.Request.Edit.ID).All(a => a.Allowed) && poAcl.All(a => a.Allowed))) &&
                                                              ((int)r.Status < 500 ? true : (pAcl.Where(a => a.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditRequestMetadata.ID).Any() && pAcl.Where(a => a.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditRequestMetadata.ID).All(a => a.Allowed)))
                                                      select rdm.DataMartID).GroupBy(k => k).Select(k => k.Key).ToArrayAsync();

                }

                Logger.Debug("####### Modified #######");

                var editableRequestsQuery = from r in DataContext.Secure<Request>(_identity).AsNoTracking()
                                   let userID = _identity.ID
                                   let requestEditPermissionID = PermissionIdentifiers.Request.Edit.ID
                                   let editRequestMetadataPermissionID = PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditRequestMetadata.ID
                                   let requestEdit = DataContext.FilteredGlobalAcls(userID, requestEditPermissionID).Select(a => a.Allowed)
                                   .Concat(DataContext.FilteredProjectAcls(userID, requestEditPermissionID, r.ProjectID).Select(a => a.Allowed))
                                   .Concat(DataContext.FilteredProjectOrganizationsAcls(userID, requestEditPermissionID, r.ProjectID, r.OrganizationID).Select(a => a.Allowed))
                                   let editRequestMetadata = DataContext.FilteredProjectAcls(userID, editRequestMetadataPermissionID, r.ProjectID).Select(a => a.Allowed)
                                   .Concat(DataContext.ProjectRequestTypeWorkflowActivities.Where(a => a.PermissionID == editRequestMetadataPermissionID && a.ProjectID == r.ProjectID && a.RequestTypeID == r.RequestTypeID && a.WorkflowActivityID == r.WorkFlowActivityID && a.SecurityGroup.Users.Any(sgu => sgu.UserID == userID)).Select(a => a.Allowed))
                                   where (requestEdit.Any() && requestEdit.All(a => a))
                                   && ((int)r.Status < 500 ? true : (editRequestMetadata.Any() && editRequestMetadata.All(a => a)))
                                   select r;

                var editableDataMarts = await (from rdm in DataContext.RequestDataMarts
                                         join dm in DataContext.DataMarts on rdm.DataMartID equals dm.ID
                                         join r in editableRequestsQuery on rdm.RequestID equals r.ID
                                         let seeRequestsPermissionID = PermissionIdentifiers.DataMartInProject.SeeRequests.ID
                                         let userID = _identity.ID
                                         let datamartAcls = DataContext.FilteredGlobalAcls(userID, seeRequestsPermissionID).Select(a => a.Allowed)
                                         .Concat(DataContext.FilteredOrganizationAcls(userID, seeRequestsPermissionID, dm.OrganizationID).Select(a => a.Allowed))
                                         .Concat(DataContext.FilteredProjectAcls(userID, seeRequestsPermissionID, r.ProjectID).Select(a => a.Allowed))
                                         .Concat(DataContext.FilteredDataMartAcls(userID, seeRequestsPermissionID, rdm.DataMartID).Select(a => a.Allowed))
                                         .Concat(DataContext.FilteredProjectDataMartAcls(userID, seeRequestsPermissionID, r.ProjectID, rdm.DataMartID).Select(a => a.Allowed))
                                         where datamartAcls.Any() && datamartAcls.All(a => a)
                                         select rdm.DataMartID).Distinct().ToArrayAsync();


            }
        }

    }
}
