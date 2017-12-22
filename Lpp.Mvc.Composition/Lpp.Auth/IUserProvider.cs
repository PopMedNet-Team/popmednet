using System.Linq.Expressions;
using System;

namespace Lpp.Auth
{
    public interface IUserProvider<TUser>
         where TUser : class, IUser
    {
        TUser FindUser( string id );
        TUser GetByClaimedId( string claimedId, bool createIfAbsent );
    }
}