using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Models.Notifications
{
    /// <summary>
    /// Represents the type of change the object had.
    /// </summary>
    public enum ChangeType
    {
        /// <summary>
        /// No change to the object.
        /// </summary>
        NoChange = 0,
        /// <summary>
        /// The object was added.
        /// </summary>
        Added,
        /// <summary>
        /// The object was updated.
        /// </summary>
        Updated,
        /// <summary>
        /// The object was deleted.
        /// </summary>
        Deleted
    }
}
