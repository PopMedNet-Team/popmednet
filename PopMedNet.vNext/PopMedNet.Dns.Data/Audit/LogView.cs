using PopMedNet.Dns.DTO.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PopMedNet.Dns.Data.Audit
{
    public abstract class LogView
    {
        public DateTimeOffset TimeStamp { get; set; }
        [ReadOnly(true)]
        public Frequencies Frequency { get; set; }
        [ReadOnly(true)]
        public DateTimeOffset? NextDueTime { get; set; }
        [ReadOnly(true)]
        public DateTimeOffset? LastRunTime { get; set; }
        [Key, ReadOnly(true)]
        public Guid UserID { get; set; }
        [ReadOnly(true)]
        public string Email { get; set; }
        [ReadOnly(true)]
        public string Phone { get; set; }
        [ReadOnly(true)]
        public string UserName { get; set; }
        public string Description { get; set; }
    }
}
