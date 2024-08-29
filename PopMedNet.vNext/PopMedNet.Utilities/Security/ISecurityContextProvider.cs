using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Utilities.Security
{
    public interface ISecurityContextProvider<TPermission>
    {
        List<object> Security { get; }
        Task<IUser> ValidateUserAsync(string userName, string password);

        Task<IEnumerable<TPermission>> HasGrantedPermissions<TEntity>(ApiIdentity identity, Guid objID, params TPermission[] permissions)
            where TEntity : class;

        Task<IEnumerable<TPermission>> HasGrantedPermissions<TEntity>(ApiIdentity identity, Guid[] objIDs, params TPermission[] permissions)
            where TEntity : class;

        Task<IEnumerable<Guid>> HasGrantedPermissions<TEntity>(ApiIdentity identity, Guid[] objIDs, params Guid[] permissions)
            where TEntity : class;
    }
}
