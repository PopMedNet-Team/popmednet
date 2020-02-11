using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.QueryComposer
{
    /// <summary>
    /// Definition of a term.
    /// </summary>
    public interface IModelTerm
    {
        /// <summary>
        /// The unique id of the term.
        /// </summary>
        Guid ID { get; }
        /// <summary>
        /// The name of the term.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// A description of the term.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// An object ID for the term.
        /// </summary>
        string OID { get; }
        /// <summary>
        /// A url for the term's?
        /// </summary>
        string ReferenceUrl { get; }
    }
}
