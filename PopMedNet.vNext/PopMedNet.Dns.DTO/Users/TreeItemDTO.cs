using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Tree Item
    /// </summary>
    [DataContract]
    public class TreeItemDTO
    {
        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        [DataMember]
        public Guid? ID { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public string? Name { get; set; }
        /// <summary>
        /// Path
        /// </summary>
        [DataMember]
        public string? Path { get; set; }
        /// <summary>
        /// Gets or sets the Type
        /// </summary>
        [DataMember]
        public int Type { get; set; }
        /// <summary>
        /// Available sub items
        /// </summary>
        [DataMember]
        public IEnumerable<TreeItemDTO>? SubItems { get; set; }
        /// <summary>
        /// Set or Get flag to determine if the item has children
        /// </summary>
        [DataMember]
        public bool HasChildren { get; set; }
    }
}
