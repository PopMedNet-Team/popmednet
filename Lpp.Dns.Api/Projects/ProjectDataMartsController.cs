using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Utilities.WebSites.Controllers;
using System;
using System.Data.Entity;
using Lpp.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Api.Projects
{
    /// <summary>
    /// Controller that services the project DataMarts
    /// </summary>
    public class ProjectDataMartsController : LppApiController<DataContext>
    {
        /// <summary>
        /// Gets a list of items allowing oData Filtering criteria
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<ProjectDataMartDTO> List()
        {
            var obj = (from o in DataContext.Secure<ProjectDataMart>(Identity) select o).Map<ProjectDataMart, ProjectDataMartDTO>();

            return obj;
        }

        /// <summary>
        /// Returns Project Data Marts with their supported request types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<ProjectDataMartWithRequestTypesDTO> ListWithRequestTypes()
        {
            var results = from dm in DataContext.Secure<ProjectDataMart>(Identity)
                          let legacyRequestTypes = dm.DataMart.Models.Distinct().Select(m => m.Model.RequestTypes).SelectMany(m => m.Where(s => s.RequestType.WorkflowID.HasValue == false).Select(s => s.RequestType)).Where(lrt => dm.Project.RequestTypes.Any(prt => prt.RequestTypeID == lrt.ID))
                          let qeRequestTypes = dm.DataMart.Adapter.RequestTypes.Select(s => s.RequestType).Where(lrt => dm.Project.RequestTypes.Any(prt => prt.RequestTypeID == lrt.ID))
                          let requestTypes = DataContext.RequestTypes.Where(rr => rr.Models.Any() == false && rr.WorkflowID.HasValue).Where(lrt => dm.Project.RequestTypes.Any(prt => prt.RequestTypeID == lrt.ID))
                          select new ProjectDataMartWithRequestTypesDTO
                          {
                              DataMart = dm.DataMart.Name,
                              DataMartID = dm.DataMartID,
                              Organization = dm.DataMart.Organization.Name,
                              ProjectID = dm.ProjectID,
                              Project = dm.Project.Name,
                              ProjectAcronym = dm.Project.Acronym,
                              RequestTypes = requestTypes.Concat(qeRequestTypes).Concat(legacyRequestTypes).Distinct().Select(rt => new RequestTypeDTO
                              {
                                  AddFiles = rt.AddFiles,
                                  Description = rt.Description,
                                  ID = rt.ID,
                                  Metadata = rt.MetaData,
                                  Name = rt.Name,
                                  PostProcess = rt.PostProcess,
                                  RequiresProcessing = rt.RequiresProcessing,
                                  Timestamp = rt.Timestamp
                              }).OrderBy(rt => rt.Name)
                          };

            return results;
        }

        /// <summary>
        /// Returns a specific data mart with the support request types
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="dataMartID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ProjectDataMartWithRequestTypesDTO> GetWithRequestTypes(Guid projectID, Guid dataMartID)
        {
            var results = await (from dm in DataContext.Secure<ProjectDataMart>(Identity)
                                 let legacyRequestTypes = dm.DataMart.Models.Distinct().Select(m => m.Model.RequestTypes).SelectMany(m => m.Where(s => s.RequestType.WorkflowID.HasValue == false).Select(s => s.RequestType)).Where(lrt => dm.Project.RequestTypes.Any(prt => prt.RequestTypeID == lrt.ID))
                                 let qeRequestTypes = dm.DataMart.Adapter.RequestTypes.Select(s => s.RequestType).Where(lrt => dm.Project.RequestTypes.Any(prt => prt.RequestTypeID == lrt.ID))
                                 let requestTypes = DataContext.RequestTypes.Where(rr => rr.Models.Any() == false && rr.WorkflowID.HasValue).Where(lrt => dm.Project.RequestTypes.Any(prt => prt.RequestTypeID == lrt.ID))
                                 where dm.ProjectID == projectID && dm.DataMartID == dataMartID
                          select
                          new ProjectDataMartWithRequestTypesDTO
                          {
                              DataMart = dm.DataMart.Name,
                              DataMartID = dm.DataMartID,
                              Organization = dm.DataMart.Organization.Name,
                              ProjectID = dm.ProjectID,
                              Project = dm.Project.Name,
                              ProjectAcronym = dm.Project.Acronym,
                              RequestTypes = requestTypes.Concat(qeRequestTypes).Concat(legacyRequestTypes).Distinct().Select(rt => new RequestTypeDTO
                              {
                                  AddFiles = rt.AddFiles,
                                  Description = rt.Description,
                                  ID = rt.ID,
                                  Metadata = rt.MetaData,
                                  Name = rt.Name,
                                  PostProcess = rt.PostProcess,
                                  RequiresProcessing = rt.RequiresProcessing,
                                  Timestamp = rt.Timestamp
                              }).OrderBy(rt => rt.Name)
                          }).FirstOrDefaultAsync();

            return results;
        }

        /// <summary>
        /// Inserts or updates a list of datamarts associated with the project
        /// </summary>
        /// <param name="updateInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> InsertOrUpdate(ProjectDataMartUpdateDTO updateInfo)
        {
            var dataMarts = updateInfo.DataMarts;

            if (!await DataContext.HasPermissions<Project>(Identity, await (from p in DataContext.Secure<Project>(Identity) where p.ID == updateInfo.ProjectID select p.ID).ToArrayAsync(), PermissionIdentifiers.Project.Edit))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to alter one or more projects referenced");

            var existing = await (from dm in DataContext.ProjectDataMarts where dm.ProjectID == updateInfo.ProjectID select dm.DataMartID).ToArrayAsync();

            var newDataMartIDs = dataMarts.Where(dm => dm.ProjectID == updateInfo.ProjectID).Select(dm => dm.DataMartID).Except(existing);

            
            foreach (var dataMartID in newDataMartIDs)
            {
                DataContext.ProjectDataMarts.Add(new ProjectDataMart
                {
                    ProjectID = updateInfo.ProjectID,
                    DataMartID = dataMartID
                });
            }

            var project = await DataContext.Projects.FindAsync(updateInfo.ProjectID);
            if (DataContext.Entry(project).State == EntityState.Unchanged)
                DataContext.ForceLog(project);

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Deletes the specified data marts from the projects
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(IEnumerable<ProjectDataMartDTO> dataMarts)
        {
            var projectIDs = dataMarts.Select(dm => dm.ProjectID);

            if (!await DataContext.HasPermissions<Project>(Identity, await (from p in DataContext.Secure<Project>(Identity) where projectIDs.Contains(p.ID) select p.ID).ToArrayAsync(), PermissionIdentifiers.Project.Edit))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to alter one or more projects referenced");

            foreach (var projectID in projectIDs)
            {
                var dataMartIDs = dataMarts.Where(dm => dm.ProjectID == projectID).Select(dm => dm.DataMartID);

                var dms = await (from dm in DataContext.ProjectDataMarts where dm.ProjectID == projectID && dataMartIDs.Contains(dm.DataMartID) select dm).ToArrayAsync();

                DataContext.ProjectDataMarts.RemoveRange(dms);
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
