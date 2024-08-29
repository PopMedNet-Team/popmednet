using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace PopMedNet.Utilities.Security
{
    public class ApiIdentity : System.Security.Claims.ClaimsIdentity, IIdentity
    {
        //public Guid ID;
        public Guid ID
        {
            get
            {
                var claim = FindFirst(System.Security.Claims.ClaimTypes.Sid);
                if(claim != null)
                {
                    return Guid.Parse(claim.Value);
                }
                return Guid.Empty;
            }
        }

        public string UserName
        {
            get
            {
                var claim = FindFirst("UserName");
                if (claim != null)
                {
                    return claim.Value;
                }
                return string.Empty;
            }
        }

        public Guid? EmployerID
        {
            get
            {
                var claim = FindFirst("EmployerID");
                if(claim != null)
                {
                    if(Guid.TryParse(claim.Value, out Guid id))
                    {
                        return id;
                    }
                }

                return null;
            }

            set
            {
                var claim = FindFirst("EmployerID");
                
                if(claim != null)
                {
                    RemoveClaim(claim);                    
                }

                AddClaim(new System.Security.Claims.Claim("EmployerID", value?.ToString() ?? string.Empty, typeof(Guid?).FullName));
            }
        }

        //public string UserName {get; private set;}
        //public string Name { get; private set; }

        //public Guid? EmployerID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="SID"></param>
        /// <param name="UserName"></param>
        /// <param name="Name"></param>
        /// <param name="SecurityRoleID">The security role for accessing the API. If null then self-only</param>
        public ApiIdentity(Guid ID, string userName, string name, Guid? employerID = null) : base("PopMedNet", System.Security.Claims.ClaimTypes.Name, System.Security.Claims.ClaimTypes.Role)
        {
            //this.ID = ID;
            //this.UserName = userName;
            //this.Name = name;
            //this.EmployerID = employerID;

            AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, name));
            AddClaim(new System.Security.Claims.Claim("UserName", userName));
            AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Sid, ID.ToString(), typeof(Guid).FullName));
            AddClaim(new System.Security.Claims.Claim("EmployerID", employerID?.ToString() ?? string.Empty, typeof(Guid?).FullName));
        }

        public override string AuthenticationType
        {
            get { return "PopMedNet"; }
        }

        public override bool IsAuthenticated
        {
            get { return true; }
        }

        //TODO: if required determine work around for HttpContext.Current
        //public string Host
        //{
        //    get { return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.Port != 80 ? ":" + HttpContext.Current.Request.Url.Port : ""); }
        //}

        //public string IP
        //{
        //    get
        //    {
        //        var httpContext = HttpContext.Current;
        //        return httpContext.Request.Headers.AllKeys.Any(k => k == "X-ClientIP") ? httpContext.Request.Headers["X-ClientIP"] : httpContext.Request.UserHostAddress;
        //    }
        //}

        //public string RawUrl
        //{
        //    get { return HttpContext.Current.Request.RawUrl; }
        //}
    }
}
