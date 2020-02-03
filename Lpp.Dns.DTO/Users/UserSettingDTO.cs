using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// User settings
    /// </summary>
    [DataContract]
    public class UserSettingDTO : EntityDto
    {
        /// <summary>
        /// ID of the user
        /// </summary>
        [DataMember]
        public Guid UserID { get; set; }
        /// <summary>
        /// Key
        /// </summary>
                [DataMember]
        public string Key { get; set; }
        /// <summary>
        /// User Setting
        /// </summary>
                [DataMember]
        public string Setting { get; set; }
    }
}
