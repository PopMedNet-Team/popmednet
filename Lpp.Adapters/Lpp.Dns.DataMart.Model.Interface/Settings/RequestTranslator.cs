using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DataMart.Model.Settings
{
    [DataContract]
    public enum RequestTranslator
    {
        [EnumMember]
        ESP_PostgreSQL = 0,
        [EnumMember]
        I2B2_SQLServer = 1,
        [EnumMember]
        HQMF_PostgreSQL = 2,
        [EnumMember]
        MSDCM_ODBC = 3,         // MiniSentinel Data Checker Model for ODBC
        [EnumMember]
        MSDCM_SQLServer = 4     // MiniSentinel Data Checker Model for SQL Server
    }
}
