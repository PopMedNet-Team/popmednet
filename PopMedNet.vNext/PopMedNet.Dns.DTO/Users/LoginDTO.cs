﻿using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Login
    /// </summary>
    [DataContract]
    public class LoginDTO
    {
        /// <summary>
        /// The Name of the user
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [DataMember]
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if RememberMe is selected
        /// </summary>
        [DataMember]
        public bool RememberMe { get; set; }
        /// <summary>
        /// The IP Address of the User
        /// </summary>
        [DataMember]
        public string IPAddress { get; set; }
        /// <summary>
        /// The Enviorment the Action Came from
        /// </summary>
        [DataMember]
        public string Environment { get; set; }
    }
}
