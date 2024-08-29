using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PopMedNet.DMCS.Code;
using PopMedNet.DMCS.Models;
using PopMedNet.DMCS.PMNApi;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Controllers
{
    [Route("api/routes")]
    [ApiController, Authorize]
    public class RoutesController : BaseApiController
    {
        readonly IOptions<DMCSConfiguration> config;

        public RoutesController(Data.Model.ModelContext modelDb, IOptions<DMCSConfiguration> config, ILogger logger)
                : base(modelDb, logger.ForContext("SourceContext", typeof(RoutesController).FullName))
        {
            this.config = config;
        }

        [HttpGet]
        public async Task<IEnumerable<RoutingDTO>> GetRoutesAsync()
        {
            var userID = GetUserID();

            IEnumerable<Guid> routingIDs = null;
            //Get a list of the requests the user is able to see from PMN API
            using (var api = new PMNApiClient(config.Value.PopMedNet))
            {
                routingIDs = await api.GetRoutesForUserAsync(userID);
            }

            if (routingIDs == null || routingIDs.Any() == false)
            {
                return Enumerable.Empty<RoutingDTO>();
            }


            return await (from rdm in this.modelDb.RequestDataMarts
                          join dm in this.modelDb.DataMarts on rdm.DataMartID equals dm.ID
                          join r in this.modelDb.Requests on rdm.RequestID equals r.ID
                          join rsp in this.modelDb.Responses on rdm.ID equals rsp.RequestDataMartID
                          orderby r.Identifier descending
                          where r.SubmittedOn > DateTime.UtcNow.AddDays(-365).Date
                          && routingIDs.Contains(rdm.ID)
                          && rsp.Count == rdm.Responses.Max(rr => rr.Count)
                          select new RoutingDTO
                          {
                              ID = rdm.ID,
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
                              SubmittedBy = r.SubmittedBy,
                              RequestIdentifier = r.Identifier,
                              RespondedBy = rsp.RespondedBy,
                              RespondedDate = rsp.ResponseTime
                          }).ToArrayAsync();
        }
    }
}
