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
    [Route("api/datamarts")]
    [ApiController, Authorize]
    public class DataMartsController : BaseApiController
    {
        readonly SyncService syncService;
        readonly IOptions<DMCSConfiguration> config;

        public DataMartsController(Data.Model.ModelContext modelDb, SyncService syncService, IOptions<DMCSConfiguration> config, ILogger logger)
            : base(modelDb, logger.ForContext("SourceContext", typeof(DataMartsController).FullName))
        {
            this.syncService = syncService;
            this.config = config;
        }

        // GET: api/datamarts
        [HttpGet]
        public async Task<IEnumerable<DataMartDTO>> Get()
        {
            var userID = GetUserID();

            return await (from dm in modelDb.DataMarts.AsNoTracking()
                          where dm.Users.Any(udm => udm.UserID == userID)
                          orderby dm.Name
                          select new DataMartDTO
                          {
                              ID = dm.ID,
                              Name = dm.Name,
                              Acronym = dm.Acronym
                          }).ToArrayAsync();
        }

        // GET api/datamars/{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
        [HttpGet("{id}")]
        public async Task<DataMartDTO> Get(Guid id)
        {
            var userID = GetUserID();

            return await this.modelDb.DataMarts.AsNoTracking().Where(dm => dm.ID == id && dm.Users.Any(u => u.UserID == userID)).Select(x => new DataMartDTO
            {
                ID = x.ID,
                Name = x.Name,
                Description = x.Description,
                Acronym = x.Acronym,
                AdapterID = x.AdapterID,
                Adapter = x.Adapter,
                AutoProcess = x.AutoProcess,
                CacheDays = x.CacheDays,
                EnableExplictCacheRemoval = x.EnableExplictCacheRemoval,
                EncryptCache = x.EncryptCache
            }).FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDatamart([FromBody] DataMartDTO dto)
        {
            using (var api = PMNApiClient.CreateForUser(config.Value, Request.Cookies))
            {
                bool canConfigure = await api.CanConfigureDataMart(dto.ID);
                if (!canConfigure)
                {
                    throw new UnauthorizedAccessException("Unauthorized exception: the user does not have permission to update the datamart.");
                }
            }


            var dm = await this.modelDb.DataMarts.Where(x => x.ID == dto.ID).FirstOrDefaultAsync();

            dm.AutoProcess = dto.AutoProcess;
            dm.CacheDays = dto.CacheDays;
            dm.EnableExplictCacheRemoval = dto.EnableExplictCacheRemoval;
            dm.EncryptCache = dto.EncryptCache;

            await this.modelDb.SaveChangesAsync();

            return Ok();
        }

        [HttpGet, Route("trigger-sync")]
        public async Task TriggerSync()
        {
            await syncService.UpdateDatamarts();
        }
    }
}
