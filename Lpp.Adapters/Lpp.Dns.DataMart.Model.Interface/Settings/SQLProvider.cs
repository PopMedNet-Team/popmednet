using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DataMart.Model.Settings
{
    [DataContract]
    public enum SQLProvider
    {
        [EnumMember]
        SQLServer = 0,
        [EnumMember]
        PostgreSQL = 1,
        [EnumMember]
        Oracle = 2,
        [EnumMember]
        ODBC = 3,
        [EnumMember]
        MySQL = 4
    }
}
