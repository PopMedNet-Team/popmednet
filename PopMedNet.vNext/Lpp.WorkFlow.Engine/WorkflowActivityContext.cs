using Lpp.Workflow.Engine.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Workflow.Engine
{
    public class WorkflowActivityContext
    {
        public Guid WorkflowID { get; set; }
        public IDbWorkflowActivity Activity { get; set; }
    }
}
