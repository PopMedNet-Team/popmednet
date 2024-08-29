using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Workflow.Engine.Interfaces
{
    /// <summary>
    /// A collection of activities for a specific workflow.
    /// </summary>
    public interface IWorkflowConfiguration
    {
        /// <summary>
        /// The ID of the workflow.
        /// </summary>
        Guid ID { get; }
        /// <summary>
        /// The collection of activities associated to the workflow.
        /// </summary>
        Dictionary<Guid, Type> Activities { get; }
    }
}
