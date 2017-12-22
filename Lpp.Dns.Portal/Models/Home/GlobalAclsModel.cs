using Lpp.Security.UI;

namespace Lpp.Dns.Portal.Models
{
    public class GlobalAclsModel
    {
        public AclEditModel RequestTypesAcl { get; set; }
        public AclEditModel DataMartAcl { get; set; }
        public AclEditModel RequestAcl { get; set; }
    }
}