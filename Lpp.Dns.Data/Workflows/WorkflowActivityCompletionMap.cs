using Lpp.Objects;
using Lpp.Workflow.Engine.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("WorkflowActivityCompletionMaps")]
    public class WorkflowActivityCompletionMap : Entity, IDbWorkflowActivityCompletionMap
    {
        [Key, Column(Order = 1)]
        public Guid WorkflowID { get; set; }

        [Key, Column(Order = 2)]
        public Guid WorkflowActivityResultID { get; set; }
        public virtual WorkflowActivityResult WorkflowActivityResult { get; set; }
        [Key, Column(Order = 3)]
        public Guid SourceWorkflowActivityID { get; set; }
        public virtual WorkflowActivity SourceWorkflowActivity { get; set; }
        [Key, Column(Order = 4)]
        public Guid DestinationWorkflowActivityID { get; set; }
        public virtual WorkflowActivity DestinationWorkflowActivity { get; set; }
    }
}
