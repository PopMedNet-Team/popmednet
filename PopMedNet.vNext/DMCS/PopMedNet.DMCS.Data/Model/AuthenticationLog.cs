using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopMedNet.DMCS.Data.Model
{
    [Table("AuthenticationLogs")]
    public class AuthenticationLog
    {
        public AuthenticationLog()
        {
            ID = DatabaseEx.NewGuid();
            TimeStamp = DateTimeOffset.UtcNow;
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; }
        public bool Success { get; set; }
        [MaxLength(40)]
        public string IPAddress { get; set; }
        [MaxLength(500)]
        public string Details { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}
