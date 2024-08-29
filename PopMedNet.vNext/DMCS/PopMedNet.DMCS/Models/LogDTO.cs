using PopMedNet.DMCS.Data.Enums;
using System;

namespace PopMedNet.DMCS.Models
{
    public class LogDTO
    {
        public DateTimeOffset DateTime { get; set; }
        public LogEventLevel Level { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string Exception { get; set; }
    }
}
