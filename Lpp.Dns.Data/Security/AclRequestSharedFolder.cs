using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("AclRequestSharedFolders")]
    public class AclRequestSharedFolder : Acl
    {
        [Key, Column(Order = 3)]
        public Guid RequestSharedFolderID { get; set; }
        public virtual RequestSharedFolder RequestSharedFolder { get; set; }

    }
}
