using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Workflow.Engine.Database
{
    public interface IDbWorkflowActivity
    {
        Guid ID { get; set; }

        string Name { get; set; }        
        string Description { get; set; }
        bool Start { get; set; }
        bool End { get; set; }
    }
}
