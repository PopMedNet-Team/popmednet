using Lpp.Utilities.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web;
using System.Web.Http;


namespace Lpp.Utilities.WebSites.Controllers
{
    public class LppApiController<TDataContext> : ApiController
        where TDataContext : DbContext, new()
    {
        private TDataContext _dbContext = null;

        protected virtual TDataContext DataContext
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = new TDataContext();

                return _dbContext;
            }
        }

        protected virtual ApiIdentity Identity
        {
            get
            {
                var context = Request.GetRequestContext();


                if (context.Principal == null)
                    throw new SecurityException("User is not authenticated");

                var apiIdentity = context.Principal.Identity as ApiIdentity;
                if (apiIdentity == null)
                    throw new SecurityException("User is not properly logged in.");

                return apiIdentity;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                    _dbContext.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
