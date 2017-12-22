using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    [DataContract]
    public enum DiagnosisRelatedGroupTypes
    {
        [EnumMember, Description("MS-DRG")]
        MS_DRG = 0,
        [EnumMember, Description("CMS-DRG")]
        CMS_DRG = 1
    }
}
