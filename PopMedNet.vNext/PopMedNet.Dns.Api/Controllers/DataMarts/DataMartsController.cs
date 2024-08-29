using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopMedNet.Dns.DTO;
using PopMedNet.Dns.Data;
using Microsoft.EntityFrameworkCore;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Dns.DTO.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Options;
using PopMedNet.Objects;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Api.DataMarts
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class DataMartsController : ApiDataControllerBase<DataMart, DataMartDTO, DataContext, PermissionDefinition>
    {

        public DataMartsController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db, mapper, config)
        {
        }

        /// <summary>
        /// Returns a list of Data Marts the user has access to that are filterable using OData
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public override IActionResult List(ODataQueryOptions<DataMartDTO> options)
        {
            var query = (from dm in DataContext.Secure<DataMart>(Identity) where !dm.Deleted select dm).AsNoTracking().ProjectTo<DataMartDTO>(_mapper.ConfigurationProvider);

            var queryHelper = new Utilities.WebSites.ODataQueryHandler<DataMartDTO>(query, options);
            return Ok(queryHelper.Result());
        }

        /// <summary>
        /// Returns a list of Data Marts the user has access to using as DataMartListDTO's.
        /// </summary>
        /// <returns></returns>
        [HttpGet("listbasic")]
        public IActionResult ListBasic(ODataQueryOptions<DataMartListDTO> options)
        {
            IQueryable<DataMartListDTO> q = DataContext.Secure<DataMart>(Identity).Where(dm => !dm.Deleted).ProjectTo<DataMartListDTO>(_mapper.ConfigurationProvider);
            var queryHelper = new Utilities.WebSites.ODataQueryHandler<DataMartListDTO>(q, options);
            return Ok(queryHelper.Result());
        }

        public override Task<ActionResult<DataMartDTO>> Get(Guid ID)
        {
            return base.Get(ID);
        }

        /// <summary>
        /// Returns a list of installed models based on the data mart.
        /// </summary>
        /// <param name="dataMartID"></param>
        /// <returns></returns>
        [HttpGet("getinstalledmodelsbydatamart"), EnableQuery]
        public IQueryable<DataMartInstalledModelDTO> GetInstalledModelsByDataMart(Guid dataMartID)
        {
            var results = (from m in DataContext.DataMartModels join dm in DataContext.Secure<DataMart>(Identity) on m.DataMartID equals dm.ID where dataMartID == m.DataMartID select m).ProjectTo<DataMartInstalledModelDTO>(_mapper.ConfigurationProvider);

            return results;
        }

        /// <summary>
        /// Returns a list of DataMart types.
        /// </summary>
        /// <returns></returns>
        [HttpGet("datamarttypelist"), EnableQuery]
        public IQueryable<DataMartTypeDTO> DataMartTypeList()
        {
            return (from r in DataContext.DataMartTypes
                    select new DataMartTypeDTO
                    {
                        ID = r.ID,
                        Name = r.Name
                    }).AsQueryable();
        }

        /// <summary>
        /// Returns a list of request types based on the data mart.
        /// </summary>
        /// <param name="DataMartId"></param>
        /// <returns></returns>
        [HttpPost("getrequesttypesbydatamarts"), EnableQuery]
        public IQueryable<RequestTypeDTO> GetRequestTypesByDataMarts(IEnumerable<Guid> DataMartId)
        {
            var results = (from dm in DataContext.Secure<DataMart>(Identity)
                           let legacyRequestTypes = dm.Models.Distinct().Select(m => m.Model.RequestTypes).SelectMany(m => m.Where(s => s.RequestType.WorkflowID.HasValue == false).Select(s => s.RequestType))
                           let qeRequestTypes = dm.Adapter.RequestTypes.Select(s => s.RequestType)
                           let requestTypes = DataContext.RequestTypes.Where(rr => rr.Models.Any() == false && rr.WorkflowID.HasValue && dm.AdapterID.HasValue).AsEnumerable()
                           where DataMartId.Contains(dm.ID)
                           select legacyRequestTypes.Concat(qeRequestTypes).Concat(requestTypes))
                    .SelectMany(t => t).Distinct().ProjectTo<DTO.RequestTypeDTO>(_mapper.ConfigurationProvider);

            return results;
        }

        /// <summary>
        /// insert or update datamarts
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost("InsertOrUpdate")]
        public override async Task<IActionResult> InsertOrUpdate(IEnumerable<DataMartDTO> values)
        {
            // Since PMN4 does not allow setting server or client based DMs, we do the same for now in PMN5.
            var clientBasedTypeID = DataContext.DataMartTypes.Where(t => t.Name == "Client based").Select(t => t.ID).FirstOrDefault();
            foreach (var value in values)
            {
                value.DataMartTypeID = value.DataMartTypeID == Guid.Empty ? clientBasedTypeID : value.DataMartTypeID;
            }

            string? error = await CheckForDuplicates(values);
            if(!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }            

            return await base.InsertOrUpdate(values);
        }

        private async Task<string?> CheckForDuplicates(IEnumerable<DataMartDTO> updates)
        {
            string? error = null;

            var ids = updates.Where(u => u.ID.HasValue).Select(u => u.ID!.Value).ToArray();
            var names = updates.Select(u => u.Name).ToArray();
            var acronyms = updates.Where(u => !u.Deleted).Where(u => u.Acronym != null && u.Acronym != "").Select(u => u.Acronym).ToArray();

            if (updates.GroupBy(u => u.Acronym).Any(u => u.Count() > 1))
            {
                //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The Acronym of DataMarts must be unique."));
                error = "The Acronym of DataMarts must be unique.";
            }else if (updates.GroupBy(u => u.Name).Any(u => u.Count() > 1))
            {
                //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The Name of DataMarts must be unique."));
                error = "The Name of DataMarts must be unique.";
            }
            else if (await (from p in DataContext.DataMarts where !p.Deleted && !ids.Contains(p.ID) && names.Contains(p.Name) && acronyms.Contains(p.Acronym) select p).AnyAsync())
            {
                //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The Name and Acronym of DataMarts must be unique."));
                error = "The Name and Acronym of DataMarts must be unique.";
            }
            
            return error;
        }
    }
}
