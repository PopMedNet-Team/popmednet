using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace PopMedNet.DMCS.PMNApi.PMNDto
{
    [DataContract]
    public class ForgotPasswordDTO
    {
        /// <summary>
        /// The Name of the User
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// User Email address
        /// </summary>
        [DataMember]
        public string Email { get; set; }
    }
}
