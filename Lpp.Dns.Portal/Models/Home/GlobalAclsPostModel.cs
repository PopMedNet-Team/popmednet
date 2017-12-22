using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Dns.Portal.Models
{
    public class GlobalAclsPostModel
    {
        public string PortalAcl { get; set; }
        public string OrganizationAcl { get; set; }
        public string DataMartAcl { get; set; }
        public string UserAcl { get; set; }
        public string RequestTypesAcl { get; set; }
        public string GroupAcl { get; set; }
        public string ProjectAcl { get; set; }
        public string SharedFolderAcl { get; set; }
        public string RequestAcl { get; set; }
        public string RegistryAcl { get; set; }
    }
}