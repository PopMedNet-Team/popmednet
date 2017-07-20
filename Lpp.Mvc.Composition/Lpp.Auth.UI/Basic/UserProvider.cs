using System.ComponentModel.Composition;
using System.Linq;
using Lpp.Auth;
using Lpp.Composition;
using Lpp.Data;

namespace Lpp.Auth.Basic
{
    [PartMetadata( ExportScope.Key, TransactionScope.Id )]
    class UserProvider<TDomain, TUser, TRolesEnum> : IUserProvider<TUser>
        where TUser : class, IRoleBasedUser<TRolesEnum>, new()
    {
        [Import] public IRepository<TDomain, TUser> Users { get; set; }
        [Import] public IUnitOfWork<TDomain> UnitOfWork { get; set; }
        [Import] public RoleBasedAuthConfig<TRolesEnum> Config { get; set; }

        public TUser FindUser( string id )
        {
            return (from i in Maybe.Parse<int>( int.TryParse, id )
                    from u in Users.Find( i )
                    select u)
                    .ValueOrNull();
        }

        public TUser GetByClaimedId( string claimedId, bool createIfAbsent )
        {
            var firstUser = Users.All.Count() == 0;
            var user =
                Users.All.FirstOrDefault( u => u.Login == claimedId ) ?? 
                Users.Add( new TUser { Login = claimedId, Roles = firstUser ? Config.FirstUserRole : default( TRolesEnum ) } );
            UnitOfWork.Commit();
            return user;
        }
    }
}