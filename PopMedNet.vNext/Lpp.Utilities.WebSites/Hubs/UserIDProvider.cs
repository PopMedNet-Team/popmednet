using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities.Security;
using System.Security;
using Microsoft.AspNet.SignalR.Hubs;
using System.Data.Entity;
using Lpp.Security;

namespace Lpp.Utilities.WebSites.Hubs
{
    public class UserIDProvider<TDataContext, TPermissions> : IUserIdProvider
        where TDataContext : DbContext, ISecurityContextProvider<TPermissions>, new()
        where TPermissions : IPermissionDefinition
    {
        private TDataContext DataContext = new TDataContext();
        private static ConcurrentDictionary<string, IUser> Users = new ConcurrentDictionary<string, IUser>();

        public UserIDProvider()
        {

        }

        public string GetUserId(IRequest request)
        {
            var Auth = request.QueryString["Auth"];
            if (Auth.IsEmpty())
                throw new ArgumentNullException("The Authorization Header was not passed.");

            

            IUser user;
            Users.TryGetValue(Auth, out user);
            if (user == null)
            {
                try
                {
                    var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(Auth)).Split(':');
                    var Login = decodedToken[0];
                    var Password = decodedToken[1];
                    user = AsyncHelpers.RunSync(() => DataContext.ValidateUser(Login, Password));
                    Users.GetOrAdd(Auth, user);
                }
                catch (SecurityException se)
                {
                    throw new NotAuthorizedException("You do not have authorization to get notifications: " + se.UnwindException());
                }
            }   

            
            //Need to get Users.ID here.
            return user.ID.ToString();
        }
    }
}
