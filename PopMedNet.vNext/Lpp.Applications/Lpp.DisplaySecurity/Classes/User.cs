using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display_Security_Settings
{
    class User
    {
        public string UserName;
        public string LastName;
        public string FirstName;
        public string Email;
        public string UserID;
        public string OrganizationName;
        public string OrganizationID;
        public string GroupName;
        public string GroupID;
        public List<Project> UserProjects;
        public List<SecurityGroup> SecurityGroups;
        public List<GlobalPermission> UserGlobalPermissions;
        public List<Organization> Organizations;
        public List<DataMart> DataMarts;
        public List<Registry> Registries;
        public List<SecurityGroup> Users;

        public override string ToString()
        {
            return LastName + ", " + FirstName;
        }

        public User()
        {
            SecurityGroups = new List<SecurityGroup>();
            UserGlobalPermissions = new List<GlobalPermission>();
            UserProjects = new List<Project>();
            Organizations = new List<Organization>();
            DataMarts = new List<DataMart>();
            Registries = new List<Registry>();
            Users = new List<SecurityGroup>();
        }

        public void AddSecurityGroups(string name, string id, string path, bool member)
        {
            SecurityGroups.Add(new SecurityGroup
                {
                    SecurityGroupName = name,
                    SecurityGroupID = id,
                    Path = path,
                    IsMemberOf = member
                });
        }


        public void AddUserGlobalPermission(string name, string id, bool allowed)
        {
            UserGlobalPermissions.Add(new GlobalPermission
                {
                    GlobalPermissionName = name,
                    GlobalPermissionID = id,
                    Allowed = allowed
                });
        }

        public void AddUserProjects(string name, string id, bool member)
        {
            UserProjects.Add(new Project
            {
                ProjectName = name,
                ProjectID = id,
                IsMemberOf = member
            });
        }

        public void AddOrganizations(string name, string id)
        {
            Organizations.Add(new Organization 
            { 
                OrgName = name,
                OrgID = id
            });
        }

        public void AddDataMarts(string name, string id)
        {
            DataMarts.Add(new DataMart
            {
                DataMartName = name,
                DataMartID = id,
            });
        }

        public void AddRegistries(string name, string id)
        {
            Registries.Add(new Registry 
            { 
                RegistryName = name,
                RegistryID = id
            });
        }

        public void AddUsers(string name, string id, string path, bool member)
        {
            Users.Add(new SecurityGroup
            {
                SecurityGroupName = name,
                SecurityGroupID = id,
                Path = path,
                IsMemberOf = member
            });
        }
    }
}
