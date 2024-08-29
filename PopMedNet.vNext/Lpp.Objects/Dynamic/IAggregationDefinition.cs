using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Lpp.Objects.Dynamic
{
    /// <summary>
    /// Defines how to aggregate and project for a collection of objects.
    /// </summary>
    public interface IAggregationDefinition
    {
        /// <summary>
        /// Gets or sets the collection of property names to group by.
        /// </summary>
        IEnumerable<string> GroupBy { get; set; }
        /// <summary>
        /// Gets or sets the collection of PropertyDefinitions defining the properties to select for the aggregation result.
        /// The definition will indicate if any aggregate action should be applied.
        /// </summary>
        IEnumerable<IPropertyDefinition> Select { get; set; }
    }
}
