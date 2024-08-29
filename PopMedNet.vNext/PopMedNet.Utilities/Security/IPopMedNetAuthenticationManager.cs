using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Utilities.Security
{
    public interface IPopMedNetAuthenticationManager
    {
        bool ValidateUser(string userName, string password, out IUser user);
    }
}
