using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.QueryComposer
{
    public interface IVisualTerm
    {
        /// <summary>
        /// The globally unique ID of the term that is shared with the adapters
        /// </summary>
        Guid TermID { get; }

        /// <summary>
        /// The name to be displayed in the list of available terms
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The description of the term to be displayed
        /// </summary>
        string Description { get; }        

        /// <summary>
        /// The category that the term will be displayed under in the user interface.
        /// </summary>
        string Category { get; }
        /// <summary>
        /// The path relative to the Terms folder
        /// </summary>
        string CriteriaEditRelativePath { get; }
        /// <summary>
        /// The path relative to the Terms folder
        /// </summary>

        string CriteriaViewRelativePath { get; }
        /// <summary>
        /// The path relative to the Terms folder
        /// </summary>
        string StratifierEditRelativePath { get; }
        /// <summary>
        /// The path relative to the Terms folder
        /// </summary>
        string StratifierViewRelativePath { get; }
        /// <summary>
        /// The path relative to the Terms folder
        /// </summary>
        string ProjectionEditRelativePath { get; }
        /// <summary>
        /// The path relative to the Terms folder
        /// </summary>
        string ProjectionViewRelativePath { get; }
        /// <summary>
        /// Return the structure of the values that will be serialized and used 
        /// </summary>
        object ValueTemplate { get; }
    }
}
