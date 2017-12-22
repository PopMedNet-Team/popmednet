using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Events;
using Lpp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Audit
{
    [Table("LogsExternalCommunication")]
    public class ExternalCommunicationLog : ChangeLog
    {
        public ExternalCommunicationLog()
        {
            this.EventID = EventIdentifiers.ExternalCommunication.CommunicationFailed.ID;
            this.ID = DatabaseEx.NewGuid();
        }

        [Key, Column(Order = 3)]
        public Guid ID { get; set; }
    }
}
