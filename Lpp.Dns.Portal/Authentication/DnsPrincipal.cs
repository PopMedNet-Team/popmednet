using System.Diagnostics.Contracts;
using System.Security.Principal;
using Lpp.Dns.Data;
using Lpp.Utilities.Security;

namespace Lpp.Dns.Portal
{
    public class DnsPrincipal : IPrincipal
    {
        private readonly User _user;        

        public DnsPrincipal( User user )
        {
            _user = user;            
            ApiIdentity = new Lpp.Utilities.Security.ApiIdentity(user.ID, user.UserName, user.FullName, user.OrganizationID.Value);
        }

        public User User { 
            get 
            {
                return _user; 
            } 
        }

        public Lpp.Utilities.Security.ApiIdentity ApiIdentity
        {
            get;
            private set;
        }

        public IIdentity Identity 
        {
            get { return ApiIdentity; }
        }

        public bool IsInRole( string role ) { 
            return false; 
        }
    }
}