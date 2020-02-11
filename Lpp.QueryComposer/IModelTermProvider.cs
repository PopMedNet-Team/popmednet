using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.QueryComposer
{
    /// <summary>
    /// Interface for an IModelTerm provider.
    /// </summary>
    public interface IModelTermProvider
    {
        /// <summary>
        /// Gets the ID of the model the terms are associated with.
        /// </summary>
        Guid ModelID { get; }

        /// <summary>
        /// The collection of IModelTerms.
        /// </summary>
        IEnumerable<IModelTerm> Terms { get; }

        Guid ProcessorID { get; }

        string Processor { get; }
    }
}
