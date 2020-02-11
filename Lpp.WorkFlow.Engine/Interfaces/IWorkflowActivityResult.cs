using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Workflow.Engine.Interfaces
{
    public interface IWorkflowActivityResult
    {
        Guid ID { get; set; }
        string Uri { get; set; }
    }
}
