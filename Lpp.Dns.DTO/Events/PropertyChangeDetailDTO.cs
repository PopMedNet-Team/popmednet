using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Container for property change details.
    /// </summary>
    [DataContract]
    public class PropertyChangeDetailDTO
    {
        /// <summary>
        /// The property that was changed.
        /// </summary>
        [DataMember]
        public string Property { get; set; }
        /// <summary>
        /// Gets or set the formatted display name of the property.
        /// </summary>
        [DataMember]
        public string PropertyDisplayName { get; set; }
        /// <summary>
        /// The original value of the property.
        /// </summary>
        [DataMember]
        public object OriginalValue { get; set; }
        /// <summary>
        /// A user friendly value of the Original value.
        /// </summary>
        [DataMember]
        public string OriginalValueDisplay { get; set; }
        /// <summary>
        /// The new value the property was changed to.
        /// </summary>
        [DataMember]
        public object NewValue { get; set; }
        /// <summary>
        /// A user friendly value of the New value.
        /// </summary>
        [DataMember]
        public string NewValueDisplay { get; set; }
    }
}
