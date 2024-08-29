using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display_Security_Settings
{
    class Organization
    {
        public string OrgName;
        public string OrgID;
        public List<GlobalPermission> OrgPermissions;

        public override string ToString()
        {
            return OrgName;
        }

        public Organization() 
        {
            OrgPermissions = new List<GlobalPermission>();
        }

        public void AddPermission(string name, string id, string path, bool allowed)
        {
            OrgPermissions.Add(new GlobalPermission
            {
                GlobalPermissionName = name,
                GlobalPermissionID = id,
                Allowed = allowed
            });
        }

    }
}
