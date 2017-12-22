using System.Diagnostics.Contracts;
using System.Security.Principal;

namespace Lpp.Auth
{
    public class Principal<TUser> : IPrincipal where TUser : class, IUser
    {
        private readonly TUser _user;
        public TUser User { get { return _user; } }

        public Principal( TUser user )
        {
            //Contract.Requires( user != null );
            _user = user;
            Identity = new GenericIdentity( user.Login );
        }

        public IIdentity Identity { get; private set; }
        public bool IsInRole( string role ) { return false; }
    }
}