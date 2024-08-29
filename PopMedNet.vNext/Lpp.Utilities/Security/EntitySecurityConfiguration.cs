using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities.Security
{
    public abstract class EntitySecurityConfiguration<TDataContext, TEntity, TPermission>
        where TEntity : class 
        where TDataContext : DbContext
    {
        public abstract IQueryable<TEntity> SecureList(TDataContext db, IQueryable<TEntity> query, ApiIdentity identity,
            params TPermission[] permissions);

        public abstract Task<bool> CanInsert(TDataContext db, ApiIdentity identity, params TEntity[] objs);

        public abstract Task<bool> CanDelete(TDataContext db, ApiIdentity identity, params Guid[] keys);

        public abstract Task<bool> CanUpdate(TDataContext db, ApiIdentity identity, params Guid[] keys);

        public Task<IEnumerable<TPermission>> HasGrantedPermissions(TDataContext db, ApiIdentity identity, Guid objID, params TPermission[] permissions)
        {
            return HasGrantedPermissions(db, identity, new Guid[] { objID }, permissions);
        }

        public virtual Task<IEnumerable<TPermission>> HasGrantedPermissions(TDataContext db, ApiIdentity identity, Guid[] objID, params TPermission[] permissions)
        {
            throw new NotImplementedException("HasPermissions has not be implemented on this class");
        }

        /// <summary>
        /// Returns if a security configuration and object has ALL of the permissions passed
        /// </summary>
        /// <param name="db"></param>
        /// <param name="identity"></param>
        /// <param name="objID"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public Task<bool> HasPermissions(TDataContext db, ApiIdentity identity, Guid objID, params TPermission[] permissions)
        {
            return HasPermissions(db, identity, new Guid[] { objID }, permissions);
        }

        /// <summary>
        /// Returns if a security configuration and objects passed has ALL of the permissions passed
        /// </summary>
        /// <param name="db"></param>
        /// <param name="identity"></param>
        /// <param name="objID"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public virtual async Task<bool> HasPermissions(TDataContext db, ApiIdentity identity, Guid[] objID, params TPermission[] permissions)
        {
            var perms = await this.HasGrantedPermissions(db, identity, objID, permissions);

            return perms.Count() == permissions.Count();
        }

        /// <summary>
        /// Determines if a user has a specific permission irrespective of the data
        /// </summary>
        /// <param name="db"></param>
        /// <param name="permission"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public Task<bool> HasPermissions(TDataContext db, ApiIdentity identity, params TPermission[] permissions)
        {
            return this.HasPermissions(db, identity, new Guid[] { }, permissions);
        }
    }
}
