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
    [Table("LogsRequestAssignmentChange")]
    public class RequestAssignmentChangeLog : ChangeLog
    {
        public RequestAssignmentChangeLog()
        {
            this.EventID = EventIdentifiers.Request.RequestAssignmentChange.ID;
        }

        [Key, Column(Order = 3)]
        public Guid RequestID { get; set; }
        public virtual Request Request { get; set; }

        [Key, Column(Order = 4)]
        public Guid RequestUserUserID { get; set; }
        public virtual User RequestUserUser { get; set; }

        [Key, Column(Order = 5)]
        public Guid WorkflowRoleID { get; set; }
        public virtual WorkflowRole WorkflowRole { get; set; }
    }
}
