using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Workflow.Engine
{
    public interface IVisualWorkflowActivity
    {
        Guid WorkflowActivityID { get; }

        Guid? WorkflowID { get; }
        string Name { get; }
        /// <summary>
        /// The relative path from the ~/areas/workflow/requests/ folder
        /// </summary>
        string Path { get; }
        /// <summary>
        /// The relative path from the ~/areas/ folder of a partial view which renders the request details overview 
        /// </summary>
        string OverviewPath { get; }
    }
}
