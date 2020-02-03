using Lpp.Dns.DTO.Events;
using Lpp.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Audit
{
    [Table("LogsRequestMetadataChange")]
    public class RequestMetadataChangeLog : AuditLog
    {

        public RequestMetadataChangeLog()
        {
            this.EventID = EventIdentifiers.Request.MetadataChange.ID;
        }

        [Key, Column(Order = 3)]
        public Guid RequestID { get; set; }
        public virtual Request Request { get; set; }

        public Guid? TaskID { get; set; }
        public virtual PmnTask Task { get; set; }

        /// <summary>
        /// Contains a json object defining the individual property changes done.
        /// </summary>
        [MaxLength, Required]
        public string ChangeDetail { get; set; }

    }

    
}
