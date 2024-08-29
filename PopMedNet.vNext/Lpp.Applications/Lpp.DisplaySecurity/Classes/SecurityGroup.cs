using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display_Security_Settings
{
    class SecurityGroup
    {
        public string SecurityGroupName;
        public string SecurityGroupID;
        public string Path;
        public bool IsMemberOf;

        public override string ToString()
        {
            return Path;
        }
    }
}
