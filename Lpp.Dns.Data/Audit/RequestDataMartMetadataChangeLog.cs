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
    [Table("LogsRequestDataMartMetadataChange")]
    public class RequestDataMartMetadataChangeLog : AuditLog
    {
        
        public RequestDataMartMetadataChangeLog()
        {
            this.EventID = EventIdentifiers.Request.DataMartMetadataChange.ID;
        }

        [Key, Column(Order = 3)]
        public Guid RequestID { get; set; }
        public virtual Request Request { get; set; }

        [Key, Column(Order = 4)]
        public Guid RequestDataMartID { get; set; }
        public virtual RequestDataMart RequestDataMart { get; set; }

        public Guid? TaskID { get; set; }
        public virtual PmnTask Task { get; set; }

        /// <summary>
        /// Contains a json object defining the individual property changes done.
        /// </summary>
        [MaxLength, Required]
        public string ChangeDetail { get; set; }
    }
}
