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
    /// Controller that supports the DataMart Installed models
    /// </summary>
    public class DataMartInstalledModelsController : LppApiController<DataContext>
    {
        static readonly Guid QueryComposerModelID = new Guid("455C772A-DF9B-4C6B-A6B0-D4FD4DD98488");

        /// <summary>
        /// Inserts or updates a list of datamarts associated with a project.
        /// </summary>
        /// <param name="updateInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> InsertOrUpdate(UpdateDataMartInstalledModelsDTO updateInfo)
        {
            // All installed models have the same datamart id.
            var datamartID = updateInfo.DataMartID;

            var datamart = await DataContext.Secure<DataMart>(Identity, PermissionIdentifiers.DataMart.Edit).FirstOrDefaultAsync(dm => dm.ID == datamartID);

            if(datamart == null)
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to alter one or more DataMarts referenced.");

            await DataContext.Entry(datamart).Collection(dm => dm.Models).LoadAsync();            

            var modelsToDelete = datamart.Models.Where(m => !updateInfo.Models.Any(um => um.ModelID == m.ModelID)).ToArray();
            
            if (datamart.AdapterID.HasValue)
            {
                //if an adapter has been set on the datamart, make sure that QueryComposer is the only installed model,
                //add if missing, and remove any others.

                modelsToDelete = datamart.Models.Where(m => m.ModelID != QueryComposerModelID).ToArray();
                updateInfo.Models = updateInfo.Models.Where(m => m.ModelID == QueryComposerModelID).ToArray();

                if (!updateInfo.Models.Any(m => m.ModelID == QueryComposerModelID))
                {
                    updateInfo.Models = new[] 
                    { 
                        new DataMartInstalledModelDTO
                        {
                            DataMartID = datamart.ID,
                            ModelID = QueryComposerModelID,
                            Properties = "<Properties><Property Name=\"ModelID\" Value=\"" + datamart.AdapterID + "\" /></Properties>"
                        } 
                    };
                }
            }

            if (modelsToDelete.Length > 0)
            {
                var modelsToDeleteID = modelsToDelete.Select(m => m.ModelID).ToArray();

                var datamartRequestTypeAcls = from a in DataContext.DataMartRequestTypeAcls
                                              where a.DataMartID == datamartID && 
                                                    a.RequestType.Models.Any(m => modelsToDeleteID.Contains(m.DataModelID))
                                              select a;
                DataContext.DataMartRequestTypeAcls.RemoveRange(datamartRequestTypeAcls);

                var projectDatamartRequestTypeAcls = from a in DataContext.ProjectDataMartRequestTypeAcls
                                                     where a.DataMartID == datamartID && 
                                                           a.RequestType.Models.Any(m => modelsToDeleteID.Contains(m.DataModelID)) && 
                                                          !a.RequestType.Models.Any(m => !modelsToDeleteID.Contains(m.DataModelID))
                                                     select a;
                DataContext.ProjectDataMartRequestTypeAcls.RemoveRange(projectDatamartRequestTypeAcls);

                var projectRequestTypeAcls = from a in DataContext.ProjectRequestTypeAcls
                                             where a.RequestType.Models.Any(m => modelsToDeleteID.Contains(m.DataModelID)) && 
                                                   a.Project.DataMarts.Any(dm => dm.DataMartID == datamartID) && 
                                                  !a.Project.DataMarts.Any(dm => dm.DataMartID != datamartID && 
                                                   dm.DataMart.Models.Any(m => modelsToDeleteID.Contains(m.ModelID)))
                                             select a;
                DataContext.ProjectRequestTypeAcls.RemoveRange(projectRequestTypeAcls);

                foreach (var md in modelsToDelete)
                {
                    datamart.Models.Remove(md);
                }
            }

            var modelID = updateInfo.Models.Select(m => m.ModelID).ToArray();
            var models = await DataContext.DataModels.Where(m => modelID.Contains(m.ID)).ToArrayAsync();

            foreach (var im in updateInfo.Models)
            {
                var modelInfo = datamart.Models.FirstOrDefault(m => m.ModelID == im.ModelID);
                if (modelInfo == null)
                {
                    modelInfo = new DataMartInstalledModel
                    {
                        DataMartID = datamart.ID,
                        ModelID = im.ModelID,
                        Properties = im.Properties
                    };
                    datamart.Models.Add(modelInfo);
                }
                else
                {
                    modelInfo.Properties = im.Properties;
                }

                var model = models.Single(m => m.ID == modelInfo.ModelID);
                if (model.QueryComposer)
                {
                    //make sure the ModelID property has been set to the datamart.AdapterID

                    if (string.IsNullOrEmpty(modelInfo.Properties))
                    {
                        modelInfo.Properties = "<Properties><Property Name=\"ModelID\" Value=\"" + datamart.AdapterID + "\" /></Properties>";
                    }
                    else
                    {
                        System.Xml.Linq.XElement doc = System.Xml.Linq.XElement.Parse(modelInfo.Properties);
                        var modelNode = doc.Descendants("Property").Where(n => n.Attributes("Name").Any(a => a.Value == "ModelID")).FirstOrDefault();
                        if (modelNode != null)
                        {
                            modelNode.Attribute("Value").Value = datamart.AdapterID.HasValue ? datamart.AdapterID.Value.ToString() : string.Empty;
                        }
                        else
                        {
                            doc.Add(new System.Xml.Linq.XElement("Property", new System.Xml.Linq.XAttribute("Name", "ModelID"), new System.Xml.Linq.XAttribute("Value", datamart.AdapterID)));
                        }

                        modelInfo.Properties = doc.ToString();
                    }

                }
                
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Deletes the specified data marts from the projects
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> Remove(IEnumerable<DataMartInstalledModelDTO> models)
        {
            var datamartIDs = models.Select(m => m.DataMartID);

            if (!await DataContext.HasPermissions<DataMart>(Identity, await (from dm in DataContext.Secure<DataMart>(Identity) where datamartIDs.Contains(dm.ID) select dm.ID).ToArrayAsync(), PermissionIdentifiers.DataMart.Edit))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to alter one or more datamarts referenced");

            foreach (var datamartID in datamartIDs)
            {
                var modelIDs = models.Where(m => m.DataMartID == datamartID).Select(m => m.ModelID);

                var dms = await (from dm in DataContext.DataMartModels where dm.DataMartID == datamartID && modelIDs.Contains(dm.ModelID) select dm).ToArrayAsync();

                DataContext.DataMartModels.RemoveRange(dms);
            }

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
