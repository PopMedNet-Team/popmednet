using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Dns.DTO.Enums
{
    // TODO 
    /// <summary>
    /// Document Kind
    /// </summary>
    public static class DocumentKind
    {
        /// <summary>
        /// Document kind request
        /// </summary>
        public const string Request = "42479CA9-009D-4483-904C-89E9E7CF436E";
        /// <summary>
        /// Document kind user
        /// </summary>
        public const string User = "CD9DBBC7-AC17-48C7-ABBB-2D9ADD17E158";
        /// <summary>
        /// Document kind link
        /// </summary>
        public const string Link = "CA2F3102-1725-4C17-84FB-B8C62C460CC5";   
        /// <summary>
        /// Document kind system generated - dont log
        /// </summary>
        public const string SystemGeneratedNoLog = "System_Generated_Not_Logged";
    }
}
