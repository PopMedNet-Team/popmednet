using PopMedNet.DMCS.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopMedNet.DMCS.Data.Model
{
    [Table("Logs")]
    public class Log
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public LogEventLevel Level { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public Guid? ResponseID { get; set; }
        public Response Response { get; set; }
        //public Guid? UserID { get; set; }
        //public User User { get; set; }
    }
}
