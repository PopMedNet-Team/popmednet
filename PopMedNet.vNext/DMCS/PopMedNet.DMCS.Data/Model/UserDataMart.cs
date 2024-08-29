using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopMedNet.DMCS.Data.Model
{
    /// <summary>
    /// Represents the associations between a User and a DataMart.
    /// </summary>
    [Table("UserDataMarts")]
    public class UserDataMart
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// Gets or sets the User.
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// Gets or sets the DataMart ID.
        /// </summary>
        public Guid DataMartID { get; set; }
        /// <summary>
        /// Gets or sets the DataMart.
        /// </summary>
        public DataMart DataMart { get; set; }

    }
}
