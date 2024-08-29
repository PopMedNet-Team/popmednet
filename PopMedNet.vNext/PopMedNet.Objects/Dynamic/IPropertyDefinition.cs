using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PopMedNet.Objects.Dynamic
{
    /// <summary>
    /// Defines a property of a response item.
    /// </summary>
    public interface IPropertyDefinition
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// The name of the type of the property. This should be a defined type.
        /// </summary>
        string Type { get; set; }
        /// <summary>
        /// The name of the property to be used in a select or aggregation.
        /// </summary>
        string As { get; set; }
        /// <summary>
        /// The name of an applicable aggregation to apply when aggregating the results. (Sum, Average, Count, etc.)
        /// </summary>
        string Aggregate { get; set; }
        /// <summary>
        /// Returns the Type property as a System.Type.
        /// </summary>
        /// <returns></returns>
        Type AsType();
    }
}
