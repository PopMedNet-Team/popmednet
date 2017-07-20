using System;
using Lpp.Dns.Data;
using Lpp.Mvc;

namespace Lpp.Dns.Portal.Models
{
    public class GroupEditPostModel : CrudPostModel<Group>
    {
        public string Name { get; set; }
        public string Acl { get; set; }
        public string Organizations { get; set; }
    }
}