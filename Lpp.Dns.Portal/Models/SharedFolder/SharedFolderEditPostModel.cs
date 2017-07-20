using System;
using Lpp.Dns.Data;
using Lpp.Mvc;

namespace Lpp.Dns.Portal.Models
{
    public class SharedFolderEditPostModel : ICrudPostModel<RequestSharedFolder>
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Acl { get; set; }
        public string ReturnTo { get; set; }
        public string Save { get; set; }
        public string Delete { get; set; }
    }
}