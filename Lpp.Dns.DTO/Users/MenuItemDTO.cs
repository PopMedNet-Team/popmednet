using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Menu items
    /// </summary>
    [DataContract]
    public class MenuItemDTO
    {
        /// <summary>
        /// Menu item DTO
        /// </summary>
        public MenuItemDTO()
        {
            encoded = false;
            content = null;
        }
        /// <summary>
        /// Text
        /// </summary>

        [DataMember]
        public string text { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        [DataMember]
        public string url { get; set; }
        /// <summary>
        /// Gets or sets the indicator in encoded form
        /// </summary>
        [DataMember]
        public bool encoded { get; set; }
        /// <summary>
        /// Content
        /// </summary>
        [DataMember]
        public string content { get; set; }
        /// <summary>
        /// Available items
        /// </summary>
        [DataMember]
        public IEnumerable<MenuItemDTO> items { get; set; }
    }
}
