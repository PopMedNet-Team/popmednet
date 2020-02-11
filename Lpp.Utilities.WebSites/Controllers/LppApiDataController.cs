using Lpp.Utilities.Objects;
using Lpp.Utilities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;
using System.Security;
using Lpp.Utilities.WebSites.Controllers;
using Lpp.Objects;
using Lpp.Security;


namespace Lpp.Utilities.WebSites.Controllers
{

    public abstract class LppApiDataController<TEntity, TDto, TDataContext, TPermissions> : LppApiController<TDataContext>
        where TEntity: EntityWithID
        where TDto: EntityDtoWithID, new()
        where TDataContext : DbContext, ISecurityContextProvider<TPermissions>, new()
        where TPermissions: IPermissionDefinition
    {

        /// <summary>
        /// Returns the permissions that are available for the given object keys
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="PermissionIdentifierss"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<Guid[]> GetPermissions([FromUri]IEnumerable<Guid> IDs, [FromUri]IEnumerable<Guid> permissions)
        {
            var results = await DataContext.HasGrantedPermissions<TEntity>(Identity, IDs.ToArray(), permissions.ToArray());

            return results.ToArray();
        }

        /// <summary>
        /// Gets an item of the specific ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<TDto> Get(Guid ID)
        {
            var obj = await (from o in DataContext.Secure<TDataContext, TEntity, TPermissions>(Identity) where o.ID == ID select o).Distinct().AsNoTracking().Map<TEntity, TDto>().FirstOrDefaultAsync();

            if (obj == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Could not locate the specified item or you do not have permission to view it."));

            return obj;
        }

        /// <summary>
        /// Gets a list of items allowing oData Filtering criteria
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual IQueryable<TDto> List()
        {
            var obj = (from o in DataContext.Secure<TDataContext, TEntity, TPermissions>(Identity) select o).AsNoTracking().Map<TEntity, TDto>();

            return obj;
        }

        /// <summary>
        /// Allows insert and update in a single operation
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<IEnumerable<TDto>> InsertOrUpdate(IEnumerable<TDto> values)
        {
            if (!values.Any())
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "No Items passed." });

            Dictionary<TDto, TEntity> map;
            try
            {
                map = await LoadDTOs(values);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }

            try
            {
                await DataContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbe)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, dbe.UnwindException()));
            }
            catch (DbEntityValidationException ve)
            {
                string validationErrors = string.Join("<br/>", ve.EntityValidationErrors.Select(v => string.Join("<br/>", v.ValidationErrors.Select(e => e.PropertyName + ": " + e.ErrorMessage).ToArray())).ToArray());

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, validationErrors));
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }

            UpdateDTOs(ref map);

            return map.Select(m => m.Key);
        }

        /// <summary>
        /// Updates all items pass subject to security
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPut]
        public virtual async Task<IEnumerable<TDto>> Update(IEnumerable<TDto> values)
        {
            if (!values.Any())
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No Items passed."));

            if (values.Where(v => v.ID == null).Any())
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "All items must have a valid key"));

            //Add check for timestamp here
            Dictionary<TDto, TEntity> map;
            try
            {
                map = await LoadDTOs(values);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }

            try
            {
                await DataContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbe)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, dbe.UnwindException()));
            }
            catch (DbEntityValidationException ve)
            {
                string validationErrors = string.Join("<br/>", ve.EntityValidationErrors.Select(v => string.Join("<br/>", v.ValidationErrors.Select(e => e.PropertyName + ": " + e.ErrorMessage).ToArray())).ToArray());

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, validationErrors));
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }

            UpdateDTOs(ref map);

            return map.Select(m => m.Key);

        }

        /// <summary>
        /// Inserts all DTOs passed subject to security.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<IEnumerable<TDto>> Insert(IEnumerable<TDto> values)
        {
            if (!values.Any())
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "No Items passed." });

            if (values.Any(v => v.ID.HasValue))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotAcceptable) { ReasonPhrase = "You may not specify an Primary Key ID value on insert." });

            //Add check for timestamp here
            Dictionary<TDto, TEntity> map;
            try
            {
                map = await LoadDTOs(values);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }

            try
            {
                await DataContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbe)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, dbe.UnwindException()));
            }
            catch (DbEntityValidationException ve)
            {
                string validationErrors = string.Join("<br/>", ve.EntityValidationErrors.Select(v => string.Join("<br/>", v.ValidationErrors.Select(e => e.PropertyName + ": " + e.ErrorMessage).ToArray())).ToArray());

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, validationErrors));
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }

            UpdateDTOs(ref map);

            return map.Select(m => m.Key);
        }

        [HttpDelete]
        public virtual async Task Delete([FromUri] IEnumerable<Guid> ID)
        {
            try
            {
                var dbSet = DataContext.Set<TEntity>();

                if (!await DataContext.CanDelete<TDataContext, TEntity, TPermissions>(Identity, ID.ToArray()))
                    throw new SecurityException("We're sorry but you do not have permission to delete one or more of these items.");

                var objs = (from o in dbSet where ID.Contains(o.ID) select o);

                foreach (var obj in objs)
                {
                    dbSet.Remove(obj);
                }

                await DataContext.SaveChangesAsync();
            }
            catch (System.Security.SecurityException se)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, se));
            }
            catch (DbUpdateException dbe)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, dbe.UnwindException()));
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        /// <summary>
        /// Updates the DTOs with any database calculated values.
        /// </summary>
        /// <param name="Map"></param>
        protected virtual void UpdateDTOs(ref Dictionary<TDto, TEntity> Map)
        {
            foreach (var item in Map)
            {
                if (item.Key.ID == null)
                    item.Key.ID = item.Value.ID;

                item.Key.Timestamp = item.Value.Timestamp;
            }
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
                    var obj = dbSet.Create();
                    dbSet.Add(obj);
                    dto.ID = obj.ID;
                    objs.Add(dto, obj);

                }
            }

            if (updates.Any())
            {
                if (!await DataContext.CanUpdate<TDataContext, TEntity, TPermissions>(Identity, updates.Select(u => u.ID.Value).ToArray()))
                    throw new SecurityException("You do not have permission to update one or more of the specified items at this time.");

                var ids = updates.Select(u => u.ID.Value).ToArray();

                foreach (var obj in await (from o in DataContext.Set<TEntity>() where ids.Contains(o.ID)  select o).ToArrayAsync())
                {
                    var DTO = DTOs.First(d => d.ID.Value == obj.ID);
                    objs.Add(DTO, obj);
                }
            }

            foreach (var obj in objs)
            {
                obj.Key.Apply(DataContext.Entry(obj.Value));
            }


            var insertIDs = inserts.Select(i => i.ID.Value);
            if (insertIDs.Any() && !await DataContext.CanInsert<TDataContext, TEntity, TPermissions>(Identity, objs.Where(o => insertIDs.Contains(o.Value.ID)).Select(o => o.Value).ToArray()))
                throw new SecurityException("You do not have permission to add this type of item.");


            return objs; //Note this does not update the DTO timestamp as it must be done after save.
        }

    }
}
