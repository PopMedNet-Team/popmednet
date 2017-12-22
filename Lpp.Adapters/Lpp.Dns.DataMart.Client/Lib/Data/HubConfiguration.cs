using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Lib.Classes
{
    public class HubConfiguration
    {
        public Guid DataMartId { get; set; }
        public Guid ModelId { get; set; }
        public Guid ModelProcessorId { get; set; }
        public UnattendedMode UnattendedMode { get; set; }
        public string Properties { get; set; }
    }

    public enum UnattendedMode
    {
        NoUnattendedOperation = 0,
        NotifyOnly,
        ProcessNoUpload,
        ProcessAndUpload
    }
}
