using PopMedNet.Utilities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Utilities.Security
{
    public class AuthenticationResultModel : IUser
    {
        public AuthenticationResultModel() { }

        public AuthenticationResultModel(Guid id, string firstName, string lastOrCompanyName, string userName)
        {
            ID = id;
            FirstName = firstName;
            LastOrCompanyName = lastOrCompanyName;
            UserName = userName;
        }

        public Guid ID { get; set; }
        public string LastOrCompanyName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
