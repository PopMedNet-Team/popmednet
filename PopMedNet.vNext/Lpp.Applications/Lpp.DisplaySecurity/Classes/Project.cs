using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display_Security_Settings
{
    class Project
    {
        public string ProjectName;
        public string ProjectID;
        public bool IsMemberOf;

        public override string ToString()
        {
            return ProjectName;
        }
    }
}
