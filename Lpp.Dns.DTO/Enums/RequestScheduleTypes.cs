using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.Enums
{
    /// <summary>
    /// The type of request schedules
    /// </summary>
    [DataContract]
    public enum RequestScheduleTypes
    {
        /// <summary>
        /// Schedule task to activate the request.
        /// </summary>
        [DataMember]
        Activate = 0,

        /// <summary>
        /// Schedule task to de-activate/terminate the request schedule
        /// </summary>
        [DataMember]
        Deactivate = 1,

        /// <summary>
        /// Schedule task to execute the recurring job
        /// </summary>
        [DataMember]
        Recurring = 2
    }
}

