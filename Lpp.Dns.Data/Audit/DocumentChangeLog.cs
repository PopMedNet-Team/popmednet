using Lpp.Dns.DTO.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Audit
{
    [Table("LogsDocumentChange")]
    public class DocumentChangeLog : ChangeLog
    {
        public DocumentChangeLog()
        {
            this.EventID = EventIdentifiers.Document.Change.ID;
        }

        [Key, Column(Order = 3)]
        public Guid DocumentID { get; set; }
        public virtual Document Document { get; set; }
        [Key, Column(Order = 4)]
        public Guid ItemID { get; set; }

    }
}
