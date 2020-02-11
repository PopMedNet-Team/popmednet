using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities.WebSites.Models
{
    [DataContract]
    public class LoginResponseModelWithProject : LoginResponseModel
    {
        [DataMember]
        public Guid? ProjectID { get; set; }
    }
}
