using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities.Security
{
    [CLSCompliant(false)]
    public static class DataContextExtensions
    {
        /// <summary>
        /// Returns a secured list of entities based on the security information and user passed
        /// </summary>
        /// <typeparam name="TDataContext"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPermission"></typeparam>
        /// <param name="dataContext"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> Secure<TDataContext, TEntity, TPermission>(this TDataContext dataContext, ApiIdentity identity)
            where TDataContext: DbContext, ISecurityContextProvider<TPermission>
            where TEntity : class
        {
            DbSet<TEntity> dbSet = dataContext.Set<TEntity>();

            var sec = (EntitySecurityConfiguration<TDataContext, TEntity, TPermission>)dataContext.Security.FirstOrDefault(s => s is EntitySecurityConfiguration<TDataContext, TEntity, TPermission>);

            if (sec == null)
                throw new ArgumentNullException("sec", "The entity of type " + typeof(TEntity).Name + " does not have a security configuration");

            return sec.SecureList(dataContext, dbSet, identity);
        }

        /// <summary>
        /// Returns a secured list of entities based on the security information and user passed
        /// </summary>
        /// <typeparam name="TDataContext"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPermission"></typeparam>
        /// <param name="dataContext"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> Secure<TDataContext, TEntity, TPermission>(this IQueryable<TEntity> query, TDataContext dataContext, ApiIdentity identity)
            where TDataContext : DbContext, ISecurityContextProvider<TPermission>
            where TEntity : class
        {
            var sec = (EntitySecurityConfiguration<TDataContext, TEntity, TPermission>)dataContext.Security.FirstOrDefault(s => s is EntitySecurityConfiguration<TDataContext, TEntity, TPermission>);

            if (sec == null)
                throw new ArgumentNullException("sec", "The entity of type " + typeof(TEntity).Name + " does not have a security configuration");

            return sec.SecureList(dataContext, query, identity);
        }

        /// <summary>
        /// /// Returns a secured list of entities based on the security information, user passed and permission requested
        /// </summary>
        /// <typeparam name="TDataContext"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPermission"></typeparam>
        /// <param name="dataContext"></param>
        /// <param name="identity"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> Secure<TDataContext, TEntity, TPermission>(this TDataContext dataContext, ApiIdentity identity, params TPermission[] permissions)
            where TDataContext : DbContext, ISecurityContextProvider<TPermission>
            where TEntity : class
        {
            DbSet<TEntity> dbSet = dataContext.Set<TEntity>();

            var sec = (EntitySecurityConfiguration<TDataContext, TEntity, TPermission>)dataContext.Security.FirstOrDefault(s => s is EntitySecurityConfiguration<TDataContext, TEntity, TPermission>);

            if (sec == null)
                throw new ArgumentNullException("sec", "The entity of type " + typeof(TEntity).Name + " does not have a security configuration");

            return sec.SecureList(dataContext, dbSet, identity, permissions);
        }

        /// <summary>
        /// Returns if the current user can insert a specific type of record or not
        /// </summary>
        /// <typeparam name="TDataContext"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPermission"></typeparam>
        /// <param name="dataContext"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static async Task<bool> CanInsert<TDataContext, TEntity, TPermission>(this TDataContext dataContext, ApiIdentity identity, params TEntity[] objs)
            where TDataContext : DbContext, ISecurityContextProvider<TPermission>
            where TEntity : class
        {
            var sec = (EntitySecurityConfiguration<TDataContext, TEntity, TPermission>)dataContext.Security.FirstOrDefault(s => s is EntitySecurityConfiguration<TDataContext, TEntity, TPermission>);

            if (sec == null)
                throw new ArgumentNullException("sec", "The entity of type " + typeof(TEntity).Name + " does not have a security configuration");

            var result = await sec.CanInsert(dataContext, identity, objs);
            return result;
        }


        /// <summary>
        /// Returns if the specified user can update the specified records by keys
        /// </summary>
        /// <typeparam name="TDataContext"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPermission"></typeparam>
        /// <param name="dataContext"></param>
        /// <param name="keys"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static async Task<bool> CanUpdate<TDataContext, TEntity, TPermission>(this TDataContext dataContext, ApiIdentity identity, params Guid[] keys)
            where TDataContext : DbContext, ISecurityContextProvider<TPermission>
            where TEntity : class
        {
            var sec = (EntitySecurityConfiguration<TDataContext, TEntity, TPermission>)dataContext.Security.FirstOrDefault(s => s is EntitySecurityConfiguration<TDataContext, TEntity, TPermission>);

            if (sec == null)
                throw new ArgumentNullException("sec", "The entity of type " + typeof(TEntity).Name + " does not have a security configuration");

            var results = await sec.CanUpdate(dataContext, identity, keys);

            return results;
        }

        /// <summary>
        /// Returns if the specified user can delete the specified records by key
        /// </summary>
        /// <typeparam name="TDataContext"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPermission"></typeparam>
        /// <param name="dataContext"></param>
        /// <param name="keys"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static async Task<bool> CanDelete<TDataContext, TEntity, TPermission>(this TDataContext dataContext, ApiIdentity identity, params Guid[] keys)
            where TDataContext : DbContext, ISecurityContextProvider<TPermission>
            where TEntity : class
        {
            var sec = (EntitySecurityConfiguration<TDataContext, TEntity, TPermission>)dataContext.Security.FirstOrDefault(s => s is EntitySecurityConfiguration<TDataContext, TEntity, TPermission>);

            if (sec == null)
                throw new ArgumentNullException("sec", "The entity of type " + typeof(TEntity).Name + " does not have a security configuration");


            var results = await sec.CanDelete(dataContext, identity, keys);

            return results;
        }

        /// <summary>
        /// Returns if a specific user has permission on a specific permission type for the speicfied objs.
        /// </summary>
        /// <typeparam name="TDataContext"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPermission"></typeparam>
        /// <param name="dataContext"></param>
        /// <param name="permissions"></param>
        /// <param name="identity"></param>
        /// <param name="objs"></param>
        /// <returns></returns>

        public static async Task<bool> HasPermissions<TDataContext, TEntity, TPermission>(this TDataContext dataContext, ApiIdentity identity, Guid[] objIDs, params TPermission[] permissions)
            where TDataContext : DbContext, ISecurityContextProvider<TPermission>
            where TEntity: class
        {
            var sec = (EntitySecurityConfiguration<TDataContext, TEntity, TPermission>)dataContext.Security.FirstOrDefault(s => s is EntitySecurityConfiguration<TDataContext, TEntity, TPermission>);

            if (sec == null)
                throw new ArgumentNullException("sec", "The entity of type " + typeof(TEntity).Name + " does not have a security configuration");

            return await sec.HasPermissions(dataContext, identity, objIDs, permissions);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDataContext"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPermission"></typeparam>
        /// <param name="dataContext"></param>
        /// <param name="obj"></param>
        /// <param name="permission"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static Task<bool> HasPermissions<TDataContext, TEntity, TPermission>(this TDataContext dataContext, ApiIdentity identity, Guid objID, params TPermission[] permissions)
            where TDataContext : DbContext, ISecurityContextProvider<TPermission>
            where TEntity : class
        {
            return HasPermissions<TDataContext, TEntity, TPermission>(dataContext, identity, new Guid[] { objID }, permissions);
        }

        /// <summary>
        /// Returns permission for the specific entity type based on the specified user
        /// </summary>
        /// <typeparam name="TDataContext"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPermission"></typeparam>
        /// <param name="dataContext"></param>
        /// <param name="permissions"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static async Task<bool> HasPermissions<TDataContext, TEntity, TPermission>(this TDataContext dataContext, ApiIdentity identity, params TPermission[] permissions)
            where TDataContext : DbContext, ISecurityContextProvider<TPermission>
            where TEntity : class
        {
            return await HasPermissions<TDataContext, TEntity, TPermission>(dataContext, identity, new Guid[] { }, permissions);
        }

    }
}
