using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.DataMartClient.Enums
{
    [DataContract]
    public enum UnattendedModes
    {
        [EnumMember, Description("No Unattended Operation")]
        NoUnattendedOperation = 0,
        [EnumMember, Description("Notify Only")]
        NotifyOnly = 1,
        [EnumMember, Description("Process; No Upload")]
        ProcessNoUpload = 2,
        [EnumMember, Description("Process And Upload")]
        ProcessAndUpload = 3
    }
}
