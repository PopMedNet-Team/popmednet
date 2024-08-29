using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Api
{
    public abstract class ApiControllerBase<TDataContext> : ControllerBase
        where TDataContext : DbContext
    {
        readonly TDataContext _dbContext;

        protected ApiControllerBase(TDataContext dataContext){
            _dbContext = dataContext;
        }

        protected virtual TDataContext DataContext { get { return _dbContext; } }

        protected virtual ApiIdentity Identity
        {
            get
            {
                var context = Request.HttpContext;

                if(context.User.Identity == null)
                {
                    throw new System.Security.SecurityException("User is not authenticated");
                }

                var apiIdentity = context.User.Identity as ApiIdentity;
                if(apiIdentity == null)
                {
                    throw new System.Security.SecurityException("User is not properly logged in");
                }

                return apiIdentity;
            }
        }        
    }
}
