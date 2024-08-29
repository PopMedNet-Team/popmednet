using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopMedNet.DMCS.Data.Model
{
    [Table("Users")]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }
        [Required, MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(400)]
        public string Email { get; set; }

        public virtual IEnumerable<AuthenticationLog> AuthenticationLogs { get; set; }
        public virtual IEnumerable<UserDataMart> DataMarts { get; set; }
    }
}
