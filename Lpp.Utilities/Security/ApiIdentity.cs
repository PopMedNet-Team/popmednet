using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Lpp.Utilities.Security
{
    public class ApiIdentity : IIdentity
    {
        public Guid ID;
        public string UserName {get; private set;}
        public string Name { get; private set; }

        public Guid? EmployerID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="SID"></param>
        /// <param name="UserName"></param>
        /// <param name="Name"></param>
        /// <param name="SecurityRoleID">The security role for accessing the API. If null then self-only</param>
        public ApiIdentity(Guid ID, string userName, string name, Guid? employerID = null)
        {
            this.ID = ID;
            this.UserName = userName;
            this.Name = name;
            this.EmployerID = employerID;
        }

        public string AuthenticationType
        {
            get { return "Basic"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Host
        {
            get { return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.Port != 80 ? ":" + HttpContext.Current.Request.Url.Port : ""); }
        }

        public string IP
        {
            get
            {
                var httpContext = HttpContext.Current;
                return httpContext.Request.Headers.AllKeys.Any(k => k == "X-ClientIP") ? httpContext.Request.Headers["X-ClientIP"] : httpContext.Request.UserHostAddress;
            }
        }

        public string RawUrl
        {
            get { return HttpContext.Current.Request.RawUrl; }
        }
    }
}
