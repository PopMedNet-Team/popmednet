using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// Comment of item Type
    /// </summary>
    [DataContract]
    public enum CommentItemTypes
    {
        /// <summary>
        ///Indicates Comment item type is Task
        /// </summary>
        [EnumMember]
        Task = 1,
        /// <summary>
        /// Indicates Comment item type is Document
        /// </summary>
        [EnumMember]
        Document = 2
    }
}
