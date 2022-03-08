using Lpp.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Audit
{
    [Table("LogsDeletedDocumentArchive")]
    public class DocumentDeleteLog: AuditLog
    {
        public Guid DocumentID { get; set; }
        public Guid ItemID { get; set; }
    }
}
