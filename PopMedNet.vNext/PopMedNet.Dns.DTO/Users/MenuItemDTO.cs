using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
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
            Encoded = false;
        }
        /// <summary>
        /// Text
        /// </summary>

        [DataMember]
        public string? Text { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        [DataMember]
        public string? Url { get; set; }
        /// <summary>
        /// Gets or sets the indicator in encoded form
        /// </summary>
        [DataMember]
        public bool Encoded { get; set; }
        /// <summary>
        /// Content
        /// </summary>
        [DataMember]
        public string? Content { get; set; }
        /// <summary>
        /// Available items
        /// </summary>
        [DataMember]
        public IEnumerable<MenuItemDTO>? Items { get; set; }
    }
}
