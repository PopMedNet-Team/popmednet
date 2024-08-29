using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Theme
    /// </summary>
    [DataContract]
    public class ThemeDTO
    {
        /// <summary>
        /// Title
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// Terms
        /// </summary>
        [DataMember]
        public string Terms { get; set; }
        /// <summary>
        /// Info
        /// </summary>
        [DataMember]
        public string Info { get; set; }
        /// <summary>
        /// Resources
        /// </summary>
        [DataMember]
        public string Resources { get; set; }
        /// <summary>
        /// Footer
        /// </summary>
        [DataMember]
        public string Footer { get; set; }
        /// <summary>
        /// Logo image
        /// </summary>
        [DataMember]
        public string LogoImage { get; set; }

        /// <summary>
        /// SystemUserConfirmationTitle
        /// </summary>
        [DataMember]
        public string SystemUserConfirmationTitle { get; set; }

        /// <summary>
        /// SystemUserConfirmationContent
        /// </summary>
        [DataMember]
        public string SystemUserConfirmationContent { get; set; }

        /// <summary>
        /// Contact Us Link
        /// </summary>
        [DataMember]
        public string ContactUsHref { get; set; }
    }
}
