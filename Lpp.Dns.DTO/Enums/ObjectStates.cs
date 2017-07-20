using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// Object states
    /// </summary>
    [DataContract]
    public enum ObjectStates
    {
        // Summary:
        //     The entity is not being tracked by the context.  An entity is in this state
        //     immediately after it has been created with the new operator or with one of
        //     the System.Data.Entity.DbSet Create methods.
        /// <summary>
        /// Detached
        /// </summary>
        [EnumMember]
        Detached = 1,
        //
        // Summary:
        //     The entity is being tracked by the context and exists in the database, and
        //     its property values have not changed from the values in the database.
        /// <summary>
        /// Unchanged
        /// </summary>
        [EnumMember]
        Unchanged = 2,
        //
        // Summary:
        //     The entity is being tracked by the context but does not yet exist in the
        //     database.
        /// <summary>
        /// Added
        /// </summary>
        [EnumMember]
        Added = 4,
        //
        // Summary:
        //     The entity is being tracked by the context and exists in the database, but
        //     has been marked for deletion from the database the next time SaveChanges
        //     is called.
        /// <summary>
        /// Deleted
        /// </summary>
        [EnumMember]
        Deleted = 8,
        //
        // Summary:
        //     The entity is being tracked by the context and exists in the database, and
        //     some or all of its property values have been modified.
        /// <summary>
        /// Modified
        /// </summary>
        [EnumMember]
        Modified = 16,
    }
}
