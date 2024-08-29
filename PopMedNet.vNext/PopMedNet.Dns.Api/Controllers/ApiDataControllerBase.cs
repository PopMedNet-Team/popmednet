using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopMedNet.Objects;
using PopMedNet.Security;
using PopMedNet.Utilities;
using PopMedNet.Utilities.Objects;
using PopMedNet.Utilities.Security;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.OData.Query;
using PopMedNet.Dns.DTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PopMedNet.Dns.Api
{
    public abstract class ApiDataControllerBase<TEntity, TDto, TDataContext, TPermissions> : ApiControllerBase<TDataContext>
        where TEntity : EntityWithID
        where TDto : EntityDtoWithID, new()
        where TDataContext : Microsoft.EntityFrameworkCore.DbContext, ISecurityContextProvider<TPermissions>
        where TPermissions : IPermissionDefinition
    {
        readonly protected IMapper _mapper;
        readonly protected IConfiguration _configuration;

        public ApiDataControllerBase(TDataContext dataContext, IMapper mapper, IConfiguration config) : base(dataContext)
        {
            _mapper = mapper;
            _configuration = config;
        }

        /// <summary>
        /// Returns the permissions that are available for the given object keys
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="PermissionIdentifierss"></param>
        /// <returns></returns>
        [HttpGet("getpermissions")]
        public virtual async Task<ActionResult<Guid[]>> GetPermissions([FromQuery] IEnumerable<Guid> IDs, [FromQuery] IEnumerable<Guid> permissions)
        {
            var results = await DataContext.HasGrantedPermissions<TEntity>(Identity, IDs.ToArray(), permissions.ToArray());

            return results.ToArray();
        }

        /// <summary>
        /// Gets an item of the specific ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("get")]
        public virtual async Task<ActionResult<TDto>> Get(Guid ID)
        {
            var obj = await (from o in DataContext.Secure<TDataContext, TEntity, TPermissions>(Identity) where o.ID == ID select o).Distinct().AsNoTracking().FirstOrDefaultAsync();

            if (obj == null)
                return NotFound();

            var result = _mapper.Map<TDto>(obj);            
            return base.Ok(result);
        }

        /// <summary>
        /// Gets a list of items allowing oData Filtering criteria
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public virtual IActionResult List(ODataQueryOptions<TDto> options)
        {
            var query = (from o in DataContext.Secure<TDataContext, TEntity, TPermissions>(Identity) select o).AsNoTracking().ProjectTo<TDto>(_mapper.ConfigurationProvider);

            var queryHelper = new Utilities.WebSites.ODataQueryHandler<TDto>(query, options);
            return Ok(queryHelper.Result());
        }

        /// <summary>
        /// Allows insert and update in a single operation
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost("InsertOrUpdate")]
        public virtual async Task<IActionResult> InsertOrUpdate(IEnumerable<TDto> values)
        {
            if (!values.Any())
                return BadRequest("No items provided");

            Dictionary<TDto, TEntity> map = new Dictionary<TDto, TEntity>();

            try
            {
                map = await DoInsertOrUpdate(values);
            }
            catch(DbUpdateException dbex)
            {
                return Unauthorized(dbex.UnwindException());
            }
            catch(System.ComponentModel.DataAnnotations.ValidationException vex)
            {
                string validationErrors = $"{ string.Join(", ", vex.ValidationResult.MemberNames) }: { vex.ValidationResult.ErrorMessage }";
                return BadRequest(validationErrors);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.UnwindException());
            }

            return Accepted(map.Select(m => m.Key));
        }

        protected virtual async Task<Dictionary<TDto, TEntity>> DoInsertOrUpdate(IEnumerable<TDto> values)
        {
            Dictionary<TDto, TEntity> map = await LoadDTOs(values);

            await DataContext.SaveChangesAsync();

            UpdateDTOs(ref map);

            return map;
        }

        protected virtual async Task<Dictionary<TDto, TEntity>> LoadDTOs(IEnumerable<TDto> DTOs)
        {
            Dictionary<TDto, TEntity> objs = new Dictionary<TDto, TEntity>();

            var inserts = DTOs.Where(d => d.ID == null).ToArray();
            var updates = DTOs.Where(d => d.ID.HasValue).ToArray();

            var dbSet = DataContext.Set<TEntity>();

            if (inserts.Any())
            {

                foreach (var dto in inserts)
                {

                    //var obj = dbSet.Create();
                    var obj = Activator.CreateInstance<TEntity>();
                    dbSet.Add(obj);
                    dto.ID = obj.ID;
                    objs.Add(dto, obj);

                }
            }

            if (updates.Any())
            {
                var ids = updates.Where(u => u.ID.HasValue).Select(u => u.ID!.Value).ToArray();
                if (!await DataContext.CanUpdate<TDataContext, TEntity, TPermissions>(Identity, ids))
                    throw new System.Security.SecurityException("You do not have permission to update one or more of the specified items at this time.");                

                foreach (var obj in await (from o in DataContext.Set<TEntity>() where ids.Contains(o.ID) select o).ToArrayAsync())
                {
                    var DTO = DTOs.First(d => d.ID!.Value == obj.ID);
                    objs.Add(DTO, obj);
                }
            }

            foreach (var obj in objs)
            {
                _mapper.Map<TDto, TEntity>(obj.Key, obj.Value);
            }


            var insertIDs = inserts.Select(i => i.ID!.Value);
            if (insertIDs.Any() && !await DataContext.CanInsert<TDataContext, TEntity, TPermissions>(Identity, objs.Where(o => insertIDs.Contains(o.Value.ID)).Select(o => o.Value).ToArray()))
                throw new System.Security.SecurityException("You do not have permission to add this type of item.");


            return objs; //Note this does not update the DTO timestamp as it must be done after save.
        }

        /// <summary>
        /// Updates the DTOs with any database calculated values.
        /// </summary>
        /// <param name="map"></param>
        protected virtual void UpdateDTOs(ref Dictionary<TDto, TEntity> map)
        {
            foreach (var item in map)
            {
                if (item.Key.ID == null)
                    item.Key.ID = item.Value.ID;

                item.Key.Timestamp = item.Value.Timestamp;
            }
        }
    }
}
