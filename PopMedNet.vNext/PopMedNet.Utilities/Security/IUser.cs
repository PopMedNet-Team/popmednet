using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Utilities.Security
{
    public interface IUser
    {
        Guid ID { get; set; }
        string LastOrCompanyName { get; set; }
        string FirstName { get; set; }
        string UserName { get; set; }
    }
}
