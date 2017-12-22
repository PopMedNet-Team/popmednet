using System.Collections.Generic;
using System.Linq;
using Lpp.Security;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal.Models
{
    public class RequestTypesAclModel
    {
        public IEnumerable<Pair<SecurityTarget,PluginRequestType>> Targets { get; set; }
        public IEnumerable<Pair<SecurityPrivilege,string>> Privileges { get; set; }
    }

    public class RequestTypesAclEditorModel
    {
        public RequestTypesAclModel Acl { get; set; }
        public string Handle { get; set; }
    }
}