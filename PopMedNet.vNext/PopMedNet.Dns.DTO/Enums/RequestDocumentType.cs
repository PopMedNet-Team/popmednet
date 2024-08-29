using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Enums
{
    /// <summary>
    /// The Type of Document for DataMarts
    /// </summary>
    [DataContract]
    public enum RequestDocumentType
    {
        /// <summary>
        /// Input is the Doucments for the DataMart to Consume 
        /// </summary>
        [DataMember]
        Input = 0,
        /// <summary>
        /// Output is the Documents for the DataMarts Response
        /// </summary>
        [DataMember]
        Output = 1,
        /// <summary>
        /// AttachmentInput is the Attachment for the Datamart to Consume
        /// </summary>
        [DataMember]
        AttachmentInput = 2,
        /// <summary>
        /// AttachmentInput is the Attachment for the the DataMarts Response
        /// </summary>
        [DataMember]
        AttachmentOutput = 3
    }
}
