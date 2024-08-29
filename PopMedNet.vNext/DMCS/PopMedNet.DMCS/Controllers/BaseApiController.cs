using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PopMedNet.DMCS.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly Data.Model.ModelContext modelDb;
        protected readonly ILogger logger;

        public BaseApiController(Data.Model.ModelContext modelDb, ILogger logger) {
            this.modelDb = modelDb;
            this.logger = logger;
        }

        protected Guid GetUserID()
        {
            return User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => new Guid(x.Value)).FirstOrDefault();
        }

        protected virtual async Task AlertRequestChanged(Guid requestDataMartID, Guid userID, IHubContext<RequestHub> requestHub)
        {
            

            var dto = await (from rdm in this.modelDb.RequestDataMarts.AsNoTracking()
                             join resp in this.modelDb.Responses.AsNoTracking() on rdm.ID equals resp.RequestDataMartID
                             join dm in this.modelDb.DataMarts.AsNoTracking() on rdm.DataMartID equals dm.ID
                             join udm in this.modelDb.UserDataMarts.AsNoTracking() on dm.ID equals udm.DataMartID
                             join r in this.modelDb.Requests.AsNoTracking() on rdm.RequestID equals r.ID
                             where rdm.ID == requestDataMartID && udm.UserID == userID && resp.Count == rdm.Responses.Max(rr => rr.Count)
                             select new RequestMetadataDTO
                             {
                                 ID = rdm.ID,
                                 ResponseID = resp.ID,
                                 DataMartName = dm.Name,
                                 DataModel = rdm.ModelText,
                                 DueDate = rdm.DueDate,
                                 MSRequestID = r.MSRequestID,
                                 RequestName = r.Name,
                                 Project = r.Project,
                                 Priority = rdm.Priority,
                                 RequestDate = r.SubmittedOn,
                                 RequestType = r.RequestType,
                                 Status = rdm.Status,
                                 ResponseMessage = resp.ResponseMessage,
                                 SubmittedBy = r.SubmittedBy,
                                 RequestIdentifier = r.Identifier,
                                 LevelOfReportAggregation = r.ReportAggregationLevel,
                                 PurposeOfUse = r.PurposeOfUse,
                                 RequestID = r.ID,
                                 RequestorCenter = r.RequestorCenter,
                                 AdditionalInstructions = r.AdditionalInstructions,
                                 Description = r.Description,

                                 TaskOrder = r.TaskOrder,
                                 Activity = r.Activity,
                                 ActivityProject = r.ActivityProject,

                                 SourceTaskOrder = r.SourceTaskOrder,
                                 SourceActivity = r.SourceActivity,
                                 SourceActivityProject = r.SourceActivityProject,
                             }).FirstOrDefaultAsync();

            await requestHub.Clients.Group(requestDataMartID.ToString("D")).SendAsync(RequestHub.EventIdentifiers.RequestDataMart_Metadata, dto);



            await requestHub.Clients.Group(RequestHub.RequestListGroupName).SendAsync(RequestHub.EventIdentifiers.RequestList_RequestListUpdated,
                new Models.Notifications.ChangeNotification<Models.RoutingDTO>(
                            Models.Notifications.ChangeType.Updated,
                            new RoutingDTO
                            {
                                ID = dto.ID,
                                DataMartName = dto.DataMartName,
                                DataModel = dto.DataModel,
                                DueDate = dto.DueDate,
                                MSRequestID = dto.MSRequestID,
                                RequestName = dto.RequestName,
                                Project = dto.Project,
                                Priority = dto.Priority,
                                RequestDate = dto.RequestDate,
                                RequestType = dto.RequestType,
                                Status = dto.Status,
                                SubmittedBy = dto.SubmittedBy,
                                RequestIdentifier = dto.RequestIdentifier
                            }
                          )
                );
        }
    }
}
