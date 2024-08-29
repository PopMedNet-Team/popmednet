using Display_Security_Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lpp.DisplaySecurity.Forms
{
    public partial class MainProgram : Form
    {
        public MainProgram()
        {
            InitializeComponent();
        }

        string UserSearch = "";
        string OrgUserSearch = "";
        string settingsUID = " teamvtpc2/handerson";
        string settingspassword = "";
        string settingsserver = "TEAMVTPC2";
        string settingsTC = "yes";
        string settingsDB = "DNS3_51";
        string settingsCTO = "30";
        User CheckedUser;
        bool allowChecking = true;

        //This method creates a new instance of the application
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainProgram newwindow = new MainProgram();
            newwindow.Show();
        }

        //This method closes the current application window
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ChangeConnection_Click(object sender, EventArgs e)
        {
            /*frmConnectionSettings newconnection = new frmConnectionSettings();
            newconnection.Show();*/
        }

        private void UserList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            for (int ix = 0; ix < UserList.Items.Count; ++ix)
                if (ix != e.Index) UserList.SetItemChecked(ix, false);
        }

        private void StopCheck(object sender, ItemCheckEventArgs e)
        {
            if (allowChecking == false)
            {
                e.NewValue = e.CurrentValue;
            }            
        }

        //When given an input into the user search box, this method finds all users that match. Capitalization matters
        private void UserFindBtn_Click(object sender, EventArgs e)
        {
            
            List<User> Users = new List<User>();
            UserSearch = UserInputBox.Text;            

            UserList.DataSource = null;
            SecurityGroupList.DataSource = null;
            GlobalPermissionsList.DataSource = null;
            ProjectList.DataSource = null;

            SqlConnection PMNConnection = new SqlConnection(String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'",
                                                            settingsUID,
                                                            settingspassword,
                                                            settingsserver,
                                                            settingsTC,
                                                            settingsDB,
                                                            settingsCTO));
            PMNConnection.Open();

                SqlDataReader usernameReader = null;
                SqlCommand usernameCommand = new SqlCommand("select * from dbo.Users", PMNConnection);
                usernameReader = usernameCommand.ExecuteReader();
            while (usernameReader.Read())
            {
                if (UserSearch == usernameReader["Username"].ToString()
                    || UserSearch == usernameReader["Email"].ToString()
                    || UserSearch == usernameReader["ID"].ToString()
                    || UserSearch == usernameReader["FirstName"].ToString()
                    || UserSearch == usernameReader["LastName"].ToString())
                {
                    //User x = new User();
                    Users.Add(new User {
                        UserName = usernameReader["Username"].ToString(),
                        UserID = usernameReader["ID"].ToString(),
                        Email = usernameReader["Email"].ToString(),
                        LastName = usernameReader["LastName"].ToString(),
                        FirstName = usernameReader["FirstName"].ToString(),
                        OrganizationID = usernameReader["OrganizationID"].ToString()
                    });                    
                }

            }
            UserList.DataSource = Users;
            PMNConnection.Close();
            
        }


        private void OrgFindBtn_Click(object sender, EventArgs e)
        {
            
            List<User> Users = new List<User>();
            OrgUserSearch = OrgInputBox.Text;

            UserList.DataSource = null;
            SecurityGroupList.DataSource = null;
            GlobalPermissionsList.DataSource = null;
            ProjectList.DataSource = null;

            SqlConnection PMNConnection = new SqlConnection(String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'",
                                                            settingsUID,
                                                            settingspassword,
                                                            settingsserver,
                                                            settingsTC,
                                                            settingsDB,
                                                            settingsCTO));
            PMNConnection.Open();
            SqlDataReader usernameReader = null;
            SqlCommand usernameCommand = new SqlCommand(String.Format("select * from dbo.Organizations where Name = '{0}'", OrgUserSearch), PMNConnection);
            usernameReader = usernameCommand.ExecuteReader();
            while (usernameReader.Read())
            {
                if (UserSearch == usernameReader["Username"].ToString()
                    || UserSearch == usernameReader["Email"].ToString()
                    || UserSearch == usernameReader["ID"].ToString()
                    || UserSearch == usernameReader["FirstName"].ToString()
                    || UserSearch == usernameReader["LastName"].ToString())
                {
                    //User x = new User();
                    Users.Add(new User
                    {
                        UserName = usernameReader["Username"].ToString(),
                        UserID = usernameReader["ID"].ToString(),
                        Email = usernameReader["Email"].ToString(),
                        LastName = usernameReader["LastName"].ToString(),
                        FirstName = usernameReader["FirstName"].ToString(),
                        OrganizationID = usernameReader["OrganizationID"].ToString()
                    });
                }

            }
            UserList.DataSource = Users;
            PMNConnection.Close();
            
        }
       
        private void SCUBtn_Click(object sender, EventArgs e)
        {                                                  
            if (UserList.CheckedIndices.Count > 1)
            {
                UsersOverloadError errorMessage = new UsersOverloadError();
                errorMessage.Show();
            }
            else if (UserList.CheckedIndices.Count == 0)
            {
                NoUsersSelectedError errorMessage = new NoUsersSelectedError();
                errorMessage.Show();
            }
            else
            {
                CheckedUser = (User)UserList.CheckedItems[0];
                this.SetOrganization();
                this.SetProjects();
                this.SetSecurityGroups();
                this.SetGlobalPermissions();
                this.SetOrganizationPermissions();
                this.SetDataMartPermissions();
                this.SetRegistryPermissions();
                this.SetUserPermissions();
                
            }
        }       


        public void SetOrganization()
        {
            
            SqlConnection PMNConnection = new SqlConnection(String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'", 
                                                            settingsUID, 
                                                            settingspassword,
                                                            settingsserver,
                                                            settingsTC, 
                                                            settingsDB, 
                                                            settingsCTO));         
           /* SqlConnection PMNConnection = new SqlConnection("user id=" + settingsUID + ";" +
                                       "password=" + settingspassword + ";server=" + settingsserver + ";" +
                                       "Trusted_Connection=" + settingsTC + ";" +
                                       "database=" + settingsDB + "; " +
                                       "connection timeout=" + settingsCTO);*/
            PMNConnection.Open();
            SqlDataReader OrgNameReader = null;
            SqlCommand OrgNameCommand = new SqlCommand(String.Format("select * from dbo.Organizations where ID = '{0}'", CheckedUser.OrganizationID), PMNConnection);
            OrgNameReader = OrgNameCommand.ExecuteReader();
            while (OrgNameReader.Read())
            {
                CheckedUser.OrganizationName = OrgNameReader["Name"].ToString();
            }
            
            OrgInputBox.Text = CheckedUser.OrganizationName;           
            PMNConnection.Close();
            
        }

        public void SetProjects()
        {
            
            string projectname = "";
            string projectid = "";
            bool memberof = false;
            SqlConnection PMNConnection = new SqlConnection(String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'",
                                                            settingsUID,
                                                            settingspassword,
                                                            settingsserver,
                                                            settingsTC,
                                                            settingsDB,
                                                            settingsCTO));
            PMNConnection.Open();
            SqlDataReader GroupIDReader = null;
            SqlCommand GroupIDCommand = new SqlCommand(String.Format("select * from dbo.OrganizationGroups where OrganizationID = '{0}'", CheckedUser.OrganizationID), PMNConnection);
            GroupIDReader = GroupIDCommand.ExecuteReader();
            while (GroupIDReader.Read())
            {
                CheckedUser.GroupID = GroupIDReader["GroupID"].ToString();
            }
            PMNConnection.Close();

            PMNConnection.Open();
            SqlDataReader GroupNameReader = null;
            SqlCommand GroupNameCommand = new SqlCommand(String.Format("select * from dbo.Groups where ID = '{0}'", CheckedUser.GroupID), PMNConnection);
            GroupNameReader = GroupNameCommand.ExecuteReader();
            while (GroupNameReader.Read())
            {
                CheckedUser.GroupName = GroupNameReader["Name"].ToString();
            }
            PMNConnection.Close();

            PMNConnection.Open();
            SqlDataReader ProjectReader = null;
            SqlCommand ProjectCommand = new SqlCommand("select * from dbo.Projects", PMNConnection);
            ProjectReader = ProjectCommand.ExecuteReader();
            while (ProjectReader.Read())
            {
                projectname = ProjectReader["Name"].ToString();
                projectid = ProjectReader["ID"].ToString();
                if (ProjectReader["GroupID"].ToString() == CheckedUser.GroupID)
                {
                    memberof = true;
                }
                else
                {
                    memberof = false;
                }
                CheckedUser.AddUserProjects(projectname, projectid, memberof);
            }
            PMNConnection.Close();

            ProjectList.DataSource = CheckedUser.UserProjects;
            for (int i = 0; i < ProjectList.Items.Count; i++)
            {
                if (CheckedUser.UserProjects[i].IsMemberOf == true)
                {
                    ProjectList.SetItemChecked(i, true);
                }
            }
            
        }

        public void SetSecurityGroups()
        {
            
            bool memberTrue = false;
            string secname = "";
            string secID = "";
            string secPath = "";
            string projectID = "";

            foreach(Project p in CheckedUser.UserProjects)
            {
                if (p.IsMemberOf == true)
                {
                    projectID = p.ProjectID;
                    break;
                }
            }
            SqlConnection PMNConnection = new SqlConnection(String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'",
                                                            settingsUID,
                                                            settingspassword,
                                                            settingsserver,
                                                            settingsTC,
                                                            settingsDB,
                                                            settingsCTO));
            PMNConnection.Open();

            SqlDataReader SecurityReader = null;
            SqlCommand SecurityCommand = new SqlCommand(String.Format("select SecurityGroups.ID, " + 
                                                                         "SecurityGroups.Name, " + 
                                                                         "SecurityGroups.Path, " +
                                                                         "SecurityGroups.OwnerID, " + 
                                                                         "SecurityGroupUsers.UserID " + 
                                                                         "from dbo.SecurityGroups left outer join dbo.SecurityGroupUsers " +
                                                                         "on SecurityGroups.ID = SecurityGroupUsers.SecurityGroupID And SecurityGroupUsers.UserID = '{0}'" +
                                                                         "where OwnerID = '{1}' or OwnerID = '{2}'", CheckedUser.UserID, CheckedUser.OrganizationID, projectID), PMNConnection);
            SecurityReader = SecurityCommand.ExecuteReader();            
            while (SecurityReader.Read())
            {
                if (SecurityReader["UserID"].ToString() == CheckedUser.UserID)
                {
                    memberTrue = true;
                }
                else
                {
                    memberTrue = false;
                }
                
                secname = SecurityReader["Name"].ToString();
                secID = SecurityReader["ID"].ToString();
                secPath = SecurityReader["Path"].ToString();
                CheckedUser.AddSecurityGroups(secname, secID, secPath, memberTrue);
            }
            PMNConnection.Close();
            SecurityGroupList.DataSource = CheckedUser.SecurityGroups;
            for (int i = 0; i < SecurityGroupList.Items.Count; i++)
            {
                if (CheckedUser.SecurityGroups[i].IsMemberOf == true)
                {
                    SecurityGroupList.SetItemChecked(i, true);
                }                
            }
            
        }

        public void SetGlobalPermissions()
        {
            
            string prevname = "";
            string previd = "";
            bool prevallowed = false;
            string name = "";
            string id = "";
            string allowed = "";

                        
            string MemberSecurityGroups = "";
            foreach (SecurityGroup g in CheckedUser.SecurityGroups)
            {
                if (g.IsMemberOf == true && MemberSecurityGroups == "")
                {
                    MemberSecurityGroups = String.Format("where SecurityGroupID = '{0}'", g.SecurityGroupID);
                }
                else if (g.IsMemberOf == true) 
                {
                    MemberSecurityGroups = String.Format("{0} or SecurityGroupID = '{1}'", MemberSecurityGroups, g.SecurityGroupID);
                }
            }
            SqlConnection PMNConnection = new SqlConnection(String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'",
                                                            settingsUID,
                                                            settingspassword,
                                                            settingsserver,
                                                            settingsTC,
                                                            settingsDB, 
                                                            settingsCTO)); 
            PMNConnection.Open();
            SqlDataReader GlobalPermissionsReader = null;
            SqlCommand GlobalPermissionsCommand = new SqlCommand(String.Format(@"select SecurityGroupID, PermissionID, Permissions.Name, Allowed  
                                                                         from dbo.AclGlobal inner join dbo.Permissions 
                                                                         on AclGlobal.PermissionID = Permissions.ID 
                                                                         {0} Order by Permissions.Name", MemberSecurityGroups), PMNConnection);
            GlobalPermissionsReader = GlobalPermissionsCommand.ExecuteReader();
            while (GlobalPermissionsReader.Read())
            {
                name = GlobalPermissionsReader["Name"].ToString();
                id = GlobalPermissionsReader["PermissionID"].ToString();
                allowed = GlobalPermissionsReader["Allowed"].ToString();
                if (allowed == "True")
                {
                    prevallowed = true;
                }
                if(name !=prevname && prevname != "")
                {
                    CheckedUser.AddUserGlobalPermission(prevname, previd, prevallowed);
                    prevallowed = false;
                }                
                prevname = name;
                previd = id;
            }
            PMNConnection.Close();
            GlobalPermissionsList.DataSource = CheckedUser.UserGlobalPermissions;
            for (int i = 0; i < GlobalPermissionsList.Items.Count; i++)
            {
                if (CheckedUser.UserGlobalPermissions[i].Allowed == true)
                {
                    GlobalPermissionsList.SetItemChecked(i, true);
                }
            }
            
        }


        public void SetOrganizationPermissions()
        {
            
            string orgname = "";
            string orgid = "";
            SqlConnection PMNConnection = new SqlConnection(String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'",
                                                            settingsUID,
                                                            settingspassword,
                                                            settingsserver,
                                                            settingsTC,
                                                            settingsDB,
                                                            settingsCTO));
            PMNConnection.Open();
            SqlDataReader OrgReader = null;
            SqlCommand OrgCommand = new SqlCommand("select * from dbo.Organizations", PMNConnection);
            OrgReader = OrgCommand.ExecuteReader();
            while (OrgReader.Read())
            {
                orgname = OrgReader["Name"].ToString();
                orgid = OrgReader["ID"].ToString();
                CheckedUser.AddOrganizations(orgname, orgid);
            }
            PMNConnection.Close();

            OrgListDropDown.DataSource = CheckedUser.Organizations;
            
        }


        private void OrgListDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            OrgPermissionList.DataSource = null;
            System.Windows.Forms.ComboBox combobox = (System.Windows.Forms.ComboBox)sender;
            string orgID = "";
            bool permissionAllowed = false;
            orgID = ((Organization)combobox.SelectedItem).OrgID;

            
            List<GlobalPermission> OrgPermissions = new List<GlobalPermission>();
            SqlConnection PMNConnection = new SqlConnection(String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'",
                                                            settingsUID,
                                                            settingspassword,
                                                            settingsserver,
                                                            settingsTC,
                                                            settingsDB,
                                                            settingsCTO));
            PMNConnection.Open();
            SqlDataReader PermissionsReader = null;
            SqlCommand PermissionsCommand = new SqlCommand(String.Format("select SecurityGroupID, PermissionID, ID, Name, OrganizationID, Allowed from dbo.ACLOrganizations inner join dbo.Permissions on PermissionID = ID where OrganizationID = '{0}'", orgID), PMNConnection);
            PermissionsReader = PermissionsCommand.ExecuteReader();
            while(PermissionsReader.Read())
            {
                foreach(SecurityGroup g in CheckedUser.SecurityGroups)
                {
                    if (g.SecurityGroupID == PermissionsReader["SecurityGroupID"].ToString())
                    {
                        if (PermissionsReader["Allowed"].ToString() == "True" && g.IsMemberOf == true)
                        {
                            permissionAllowed = true;
                        }
                        else
                        {
                            permissionAllowed = false;
                        }
                        OrgPermissions.Add(new GlobalPermission 
                        { 
                            GlobalPermissionName = PermissionsReader["Name"].ToString(),
                            GlobalPermissionID = PermissionsReader["PermissionID"].ToString(),
                            Allowed = permissionAllowed
                        });
                        break;
                    }
                }
            }
            PMNConnection.Close();
            OrgPermissionList.DataSource = OrgPermissions;
            for (int i = 0; i < OrgPermissionList.Items.Count; i++)
            {
                if (OrgPermissions[i].Allowed == true)
                {
                    OrgPermissionList.SetItemChecked(i, true);
                }
            }
            
        }

        public void SetDataMartPermissions()
        {
            
            string dmname = "";
            string dmid = "";
            SqlConnection PMNConnection = new SqlConnection(String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'",
                                                            settingsUID,
                                                            settingspassword,
                                                            settingsserver,
                                                            settingsTC,
                                                            settingsDB,
                                                            settingsCTO));
            PMNConnection.Open();
            SqlDataReader DMReader = null;
            SqlCommand DMCommand = new SqlCommand("select * from dbo.DataMarts", PMNConnection);
            DMReader = DMCommand.ExecuteReader();
            while (DMReader.Read())
            {
                dmname = DMReader["Name"].ToString();
                dmid = DMReader["ID"].ToString();
                CheckedUser.AddDataMarts(dmname, dmid);
            }
            PMNConnection.Close();

            DataMartDropDown.DataSource = CheckedUser.DataMarts;
        }

        private void DataMartDropDown_SelectedIndexChanged(object sender, EventArgs e)
        { 
            System.Windows.Forms.ComboBox DataMartCombobox = (System.Windows.Forms.ComboBox)sender;
            DataMartPermissionsList.DataSource = null;
            string datamartID = "";
            bool permissionAllowed = false;
            datamartID = ((DataMart)DataMartCombobox.SelectedItem).DataMartID;


            List<GlobalPermission> DataMartPermissions = new List<GlobalPermission>();
            SqlConnection PMNConnection = new SqlConnection(String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'",
                                                            settingsUID,
                                                            settingspassword,
                                                            settingsserver,
                                                            settingsTC,
                                                            settingsDB,
                                                            settingsCTO));
            PMNConnection.Open();
            SqlDataReader PermissionsReader = null;
            SqlCommand PermissionsCommand = new SqlCommand(String.Format("select SecurityGroupID, PermissionID, ID, Name, DataMartID, Allowed from dbo.ACLDataMarts inner join dbo.Permissions on PermissionID = ID where DataMartID = '{0}'", datamartID), PMNConnection);
            PermissionsReader = PermissionsCommand.ExecuteReader();
            while (PermissionsReader.Read())
            {
                foreach (SecurityGroup g in CheckedUser.SecurityGroups)
                {
                    if (DataMartPermissions.Count == 0)
                    {
                        if (g.SecurityGroupID == PermissionsReader["SecurityGroupID"].ToString())
                        {
                            if (PermissionsReader["Allowed"].ToString() == "True" && g.IsMemberOf == true)
                            {
                                permissionAllowed = true;
                            }
                            else
                            {
                                permissionAllowed = false;
                            }
                            DataMartPermissions.Add(new GlobalPermission
                            {
                                GlobalPermissionName = PermissionsReader["Name"].ToString(),
                                GlobalPermissionID = PermissionsReader["PermissionID"].ToString(),
                                Allowed = permissionAllowed
                            });
                            break;
                        }
                    }
                    else if (DataMartPermissions.Last().GlobalPermissionID != PermissionsReader["PermissionID"].ToString())
                    {
                        if (g.SecurityGroupID == PermissionsReader["SecurityGroupID"].ToString())
                        {
                            if (PermissionsReader["Allowed"].ToString() == "True" && g.IsMemberOf == true)
                            {
                                permissionAllowed = true;
                            }
                            else
                            {
                                permissionAllowed = false;
                            }
                            DataMartPermissions.Add(new GlobalPermission
                            {
                                GlobalPermissionName = PermissionsReader["Name"].ToString(),
                                GlobalPermissionID = PermissionsReader["PermissionID"].ToString(),
                                Allowed = permissionAllowed
                            });
                            break;
                        }
                    }
                    else if (DataMartPermissions.Last().GlobalPermissionID == PermissionsReader["PermissionID"].ToString() && DataMartPermissions.Last().Allowed == false)
                    {
                        if (g.SecurityGroupID == PermissionsReader["SecurityGroupID"].ToString())
                        {
                            if (PermissionsReader["Allowed"].ToString() == "True" && g.IsMemberOf == true)
                            {
                                DataMartPermissions.Last().Allowed = true;
                            }
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                    
                }
            }
            PMNConnection.Close();
            DataMartPermissionsList.DataSource = DataMartPermissions;
            for (int i = 0; i < DataMartPermissionsList.Items.Count; i++)
            {
                if (DataMartPermissions[i].Allowed == true)
                {
                    DataMartPermissionsList.SetItemChecked(i, true);
                }
            }
            
        }
        
        private void SetRegistryPermissions()
        {
            
            string registryname = "";
            string registryid = "";
            SqlConnection PMNConnection = new SqlConnection(String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'",
                                                            settingsUID,
                                                            settingspassword,
                                                            settingsserver,
                                                            settingsTC,
                                                            settingsDB,
                                                            settingsCTO));
            PMNConnection.Open();
            SqlDataReader RegistryReader = null;
            SqlCommand RegistryCommand = new SqlCommand("select * from dbo.Registries", PMNConnection);
            RegistryReader = RegistryCommand.ExecuteReader();
            while (RegistryReader.Read())
            {
                registryname = RegistryReader["Name"].ToString();
                registryid = RegistryReader["ID"].ToString();
                CheckedUser.AddRegistries(registryname, registryid);
            }
            PMNConnection.Close();

            RegistryListDropDown.DataSource = CheckedUser.Registries;
            
        }

        private void RegistryListDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            RegistryPermissionsList.DataSource = null;
            System.Windows.Forms.ComboBox combobox = (System.Windows.Forms.ComboBox)sender;
            string RegistryID = "";
            bool permissionAllowed = false;
            RegistryID = ((Registry)combobox.SelectedItem).RegistryID;


            List<GlobalPermission> RegistryPermissions = new List<GlobalPermission>();
            SqlConnection PMNConnection = new SqlConnection(String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'",
                                                            settingsUID,
                                                            settingspassword,
                                                            settingsserver,
                                                            settingsTC,
                                                            settingsDB,
                                                            settingsCTO));
            PMNConnection.Open();
            SqlDataReader PermissionsReader = null;
            SqlCommand PermissionsCommand = new SqlCommand(String.Format("select SecurityGroupID, PermissionID, ID, Name, RegistryID, Allowed from dbo.ACLRegistries inner join dbo.Permissions on PermissionID = ID where RegistryID = '{0}'", RegistryID), PMNConnection);
            PermissionsReader = PermissionsCommand.ExecuteReader();
            while (PermissionsReader.Read())
            {
                foreach (SecurityGroup g in CheckedUser.SecurityGroups)
                {
                    if (g.SecurityGroupID == PermissionsReader["SecurityGroupID"].ToString())
                    {
                        if (PermissionsReader["Allowed"].ToString() == "True" && g.IsMemberOf == true)
                        {
                            permissionAllowed = true;
                        }
                        else
                        {
                            permissionAllowed = false;
                        }
                        RegistryPermissions.Add(new GlobalPermission
                        {
                            GlobalPermissionName = PermissionsReader["Name"].ToString(),
                            GlobalPermissionID = PermissionsReader["PermissionID"].ToString(),
                            Allowed = permissionAllowed
                        });
                        break;
                    }
                }
            }
            PMNConnection.Close();
            RegistryPermissionsList.DataSource = RegistryPermissions;
            for (int i = 0; i < RegistryPermissionsList.Items.Count; i++)
            {
                if (RegistryPermissions[i].Allowed == true)
                {
                    RegistryPermissionsList.SetItemChecked(i, true);
                }
            }
            
        }


        private void SetUserPermissions()
        {
            
            string userpath = "";
            string username = "";
            string userid = "";
            SqlConnection PMNConnection = new SqlConnection(String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'",
                                                            settingsUID,
                                                            settingspassword,
                                                            settingsserver,
                                                            settingsTC,
                                                            settingsDB,
                                                            settingsCTO));
            PMNConnection.Open();
            SqlDataReader UserReader = null;
            SqlCommand UserCommand = new SqlCommand("select * from dbo.SecurityGroups", PMNConnection);
            UserReader = UserCommand.ExecuteReader();
            while (UserReader.Read())
            {
                username = UserReader["Name"].ToString();
                userpath = UserReader["Path"].ToString();
                userid = UserReader["ID"].ToString();
                CheckedUser.AddUsers(username, userid, userpath, false);
            }
            PMNConnection.Close();

            UserDropDown.DataSource = CheckedUser.Users;
            
        }

        private void UserDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox UserCombobox = (System.Windows.Forms.ComboBox)sender;
            UserPermissionsList.DataSource = null;
            string UserID = "";
            bool permissionAllowed = false;
            UserID = ((SecurityGroup)UserCombobox.SelectedItem).SecurityGroupID;


            List<GlobalPermission> UserPermissions = new List<GlobalPermission>();
            SqlConnection PMNConnection = new SqlConnection(String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'",
                                                            settingsUID,
                                                            settingspassword,
                                                            settingsserver,
                                                            settingsTC,
                                                            settingsDB,
                                                            settingsCTO));
            PMNConnection.Open();
            SqlDataReader PermissionsReader = null;
            SqlCommand PermissionsCommand = new SqlCommand(String.Format("select SecurityGroupID, PermissionID, ID, Name, UserID, Allowed from dbo.ACLUsers inner join dbo.Permissions on PermissionID = ID where UserID = '{0}'", UserID), PMNConnection);
            PermissionsReader = PermissionsCommand.ExecuteReader();
            while (PermissionsReader.Read())
            {
                foreach (SecurityGroup g in CheckedUser.SecurityGroups)
                {
                    if (UserPermissions.Count == 0)
                    {
                        if (g.SecurityGroupID == PermissionsReader["SecurityGroupID"].ToString())
                        {
                            if (PermissionsReader["Allowed"].ToString() == "True" && g.IsMemberOf == true)
                            {
                                permissionAllowed = true;
                            }
                            else
                            {
                                permissionAllowed = false;
                            }
                            UserPermissions.Add(new GlobalPermission
                            {
                                GlobalPermissionName = PermissionsReader["Name"].ToString(),
                                GlobalPermissionID = PermissionsReader["PermissionID"].ToString(),
                                Allowed = permissionAllowed
                            });
                            break;
                        }
                    }
                    else if (UserPermissions.Last().GlobalPermissionID != PermissionsReader["PermissionID"].ToString())
                    {
                        if (g.SecurityGroupID == PermissionsReader["SecurityGroupID"].ToString())
                        {
                            if (PermissionsReader["Allowed"].ToString() == "True" && g.IsMemberOf == true)
                            {
                                permissionAllowed = true;
                            }
                            else
                            {
                                permissionAllowed = false;
                            }
                            UserPermissions.Add(new GlobalPermission
                            {
                                GlobalPermissionName = PermissionsReader["Name"].ToString(),
                                GlobalPermissionID = PermissionsReader["PermissionID"].ToString(),
                                Allowed = permissionAllowed
                            });
                            break;
                        }
                    }
                    else if (UserPermissions.Last().GlobalPermissionID == PermissionsReader["PermissionID"].ToString() && UserPermissions.Last().Allowed == false)
                    {
                        if (g.SecurityGroupID == PermissionsReader["SecurityGroupID"].ToString())
                        {
                            if (PermissionsReader["Allowed"].ToString() == "True" && g.IsMemberOf == true)
                            {
                                UserPermissions.Last().Allowed = true;
                            }
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                }
            }
            PMNConnection.Close();
            UserPermissionsList.DataSource = UserPermissions;
            for (int i = 0; i < UserPermissionsList.Items.Count; i++)
            {
                if (UserPermissions[i].Allowed == true)
                {
                    UserPermissionsList.SetItemChecked(i, true);
                }
            }

        }
    }
}
