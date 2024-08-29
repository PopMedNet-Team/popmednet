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
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace Lpp.DisplaySecurity.Forms
{
    public partial class ReflectionForm : Form
    {
        string UserSearch;
        string OrgUserSearch;
        string settingsUID;
        string settingspassword;
        string settingsserver;
        string settingsTC;
        string settingsDB;
        string settingsCTO;
        User CheckedUser;
        bool allowChecking = true;
        string QueryString;

        public ReflectionForm()
        {
            //Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            InitializeComponent();
            string ConnectPath = "";
            ConnectPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ConnectSettings.xml");

            if (!System.IO.File.Exists(ConnectPath))
            {
                //The connection settings file doesn't exist.
                System.IO.File.WriteAllText(ConnectPath, @"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <Settings>
                                      <DefaultConnection>
                                        <UID>teamvtpc2/handerson</UID>
                                        <Password></Password>
                                        <Server>TEAMVTPC2</Server>
                                        <TrustedConnection>yes</TrustedConnection>
                                        <Database>DNS3_51</Database>
                                        <ConnectionTO>30</ConnectionTO>
                                      </DefaultConnection>
                                      <Connection>
                                        <UID>teamvtpc2/handerson</UID>
                                        <Password></Password>
                                        <Server>TEAMVTPC2</Server>
                                        <TrustedConnection>yes</TrustedConnection>
                                        <Database>DNS3_51</Database>
                                        <ConnectionTO>30</ConnectionTO>
                                      </Connection>
                                    </Settings>");
            }
            XDocument doc = XDocument.Load(ConnectPath);
            XElement defaultCon = doc.Root.Element("DefaultConnection");
            settingsUID = defaultCon.Element("UID").Value;
            settingspassword = defaultCon.Element("Password").Value;
            settingsserver = defaultCon.Element("Server").Value;
            settingsTC = defaultCon.Element("TrustedConnection").Value;
            settingsDB = defaultCon.Element("Database").Value;
            settingsCTO = defaultCon.Element("ConnectionTO").Value;
            QueryString = String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'",
                                                            settingsUID,
                                                            settingspassword,
                                                            settingsserver,
                                                            settingsTC,
                                                            settingsDB,
                                                            settingsCTO);            
        }

       

        //Opens a new instance of the app
        private void newToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ReflectionForm newwindow = new ReflectionForm();
            newwindow.Show();
        }

        //Closes the current instance of the app
        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void SettingsMenu_Click(object sender, EventArgs e)
        {
            using (frmConnectionSettings settingsForm = new frmConnectionSettings())
            {
                settingsForm.ShowDialog(this);
            }
        }

        //This event makes it so that only one user can be selected from the User List at a time.
        private void UserList_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int ix = 0; ix < UserList.Items.Count; ++ix)
                if (ix != UserList.SelectedIndex) UserList.SetItemChecked(ix, false);
        } 

        //Similar to the above event, but making all the permission checked list boxes noncheckable.
        private void GlobalPermissionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckedListBox currentCheckedList = ((CheckedListBox)sender);
            if (currentCheckedList.GetItemChecked(currentCheckedList.SelectedIndex) == true)
            {
                currentCheckedList.SetItemChecked(currentCheckedList.SelectedIndex, false);
            }
            else
            {
                currentCheckedList.SetItemChecked(currentCheckedList.SelectedIndex, true);
            }
            
        }

        

       

        //When given an input into the user search box, this method finds all users that match. Capitalization matters
        private void UserFindBtn_Click(object sender, EventArgs e)
        {
            string ConnectPath = "";
            ConnectPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ConnectSettings.xml");

            if (!System.IO.File.Exists(ConnectPath))
            {
                //The connection settings file doesn't exist.
                System.IO.File.WriteAllText(ConnectPath, @"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <Settings>
                                      <DefaultConnection>
                                        <UID>teamvtpc2/handerson</UID>
                                        <Password></Password>
                                        <Server>TEAMVTPC2</Server>
                                        <TrustedConnection>yes</TrustedConnection>
                                        <Database>DNS3_51</Database>
                                        <ConnectionTO>30</ConnectionTO>
                                      </DefaultConnection>
                                      <Connection>
                                        <UID>teamvtpc2/handerson</UID>
                                        <Password></Password>
                                        <Server>TEAMVTPC2</Server>
                                        <TrustedConnection>yes</TrustedConnection>
                                        <Database>DNS3_51</Database>
                                        <ConnectionTO>30</ConnectionTO>
                                      </Connection>
                                    </Settings>");
            }
            XDocument doc = XDocument.Load(ConnectPath);
            XElement defaultCon = doc.Root.Element("DefaultConnection");
            settingsUID = defaultCon.Element("UID").Value;
            settingspassword = defaultCon.Element("Password").Value;
            settingsserver = defaultCon.Element("Server").Value;
            settingsTC = defaultCon.Element("TrustedConnection").Value;
            settingsDB = defaultCon.Element("Database").Value;
            settingsCTO = defaultCon.Element("ConnectionTO").Value;
            QueryString = String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'",
                                                            settingsUID,
                                                            settingspassword,
                                                            settingsserver,
                                                            settingsTC,
                                                            settingsDB,
                                                            settingsCTO);      
            
            List<User> Users = new List<User>();
            UserSearch = UserInputBox.Text;

            UserList.DataSource = null;
            SecurityGroupList.DataSource = null;
            GlobalPermissionsList.DataSource = null;
            ProjectList.DataSource = null;

            SqlConnection PMNConnection = new SqlConnection(QueryString);
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

            if (Users.Count != 0)
            {
                UserList.DataSource = Users;
            }
            else
            {
                UserNotFound error = new UserNotFound();
                error.ShowDialog();
            }
            PMNConnection.Close();

        }

        //Finds users by Organization.
        private void OrgFindBtn_Click_1(object sender, EventArgs e)
        {
            string ConnectPath = "";
            ConnectPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ConnectSettings.xml");

            if (!System.IO.File.Exists(ConnectPath))
            {
                //The connection settings file doesn't exist.
                System.IO.File.WriteAllText(ConnectPath, @"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <Settings>
                                      <DefaultConnection>
                                        <UID>teamvtpc2/handerson</UID>
                                        <Password></Password>
                                        <Server>TEAMVTPC2</Server>
                                        <TrustedConnection>yes</TrustedConnection>
                                        <Database>DNS3_51</Database>
                                        <ConnectionTO>30</ConnectionTO>
                                      </DefaultConnection>
                                      <Connection>
                                        <UID>teamvtpc2/handerson</UID>
                                        <Password></Password>
                                        <Server>TEAMVTPC2</Server>
                                        <TrustedConnection>yes</TrustedConnection>
                                        <Database>DNS3_51</Database>
                                        <ConnectionTO>30</ConnectionTO>
                                      </Connection>
                                    </Settings>");
            }
            XDocument doc = XDocument.Load(ConnectPath);
            XElement defaultCon = doc.Root.Element("DefaultConnection");
            settingsUID = defaultCon.Element("UID").Value;
            settingspassword = defaultCon.Element("Password").Value;
            settingsserver = defaultCon.Element("Server").Value;
            settingsTC = defaultCon.Element("TrustedConnection").Value;
            settingsDB = defaultCon.Element("Database").Value;
            settingsCTO = defaultCon.Element("ConnectionTO").Value;
            QueryString = String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'",
                                                            settingsUID,
                                                            settingspassword,
                                                            settingsserver,
                                                            settingsTC,
                                                            settingsDB,
                                                            settingsCTO);      

            List<User> Users = new List<User>();
            OrgUserSearch = OrgInputBox.Text;

            string OrgID = null;
            UserList.DataSource = null;
            SecurityGroupList.DataSource = null;
            GlobalPermissionsList.DataSource = null;
            ProjectList.DataSource = null;

            SqlConnection PMNConnection = new SqlConnection(QueryString);
            PMNConnection.Open();
            SqlDataReader OrgIDReader = null;
            SqlCommand OrgIDCommand = new SqlCommand(String.Format("select * from dbo.Organizations where Name = '{0}'", OrgUserSearch), PMNConnection);
            OrgIDReader = OrgIDCommand.ExecuteReader();
            while (OrgIDReader.Read())
            {
                OrgID = OrgIDReader["ID"].ToString();

            }
            PMNConnection.Close();
            if (OrgID != null)
            {
                PMNConnection.Open();
                SqlDataReader usernameReader = null;
                SqlCommand usernameCommand = new SqlCommand(String.Format("select * from dbo.Users where OrganizationID = '{0}'", OrgID), PMNConnection);
                usernameReader = usernameCommand.ExecuteReader();
                while (usernameReader.Read())
                {
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
                UserList.DataSource = Users;
                PMNConnection.Close();
            }
            else
            {
                OrgNotFound error = new OrgNotFound();
                error.ShowDialog();
            }
           
        }

        //Finds the selected user's information.
        private void SCUBtn_Click_1(object sender, EventArgs e)
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
                this.UserInputBox.Text = String.Format("{0} {1}", CheckedUser.FirstName, CheckedUser.LastName);
                this.SetOrganization();
                this.SetProjects();
                this.SetSecurityGroups();
                this.SetGlobalPermissions();
                this.AutoGenerateTables();

            }
        }

        //Sets the user's organization
        public void SetOrganization()
        {
            SqlConnection PMNConnection = new SqlConnection(QueryString);
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

        //Sets User's Projects
        public void SetProjects()
        {

            string projectname = "";
            string projectid = "";
            bool memberof = false;
            SqlConnection PMNConnection = new SqlConnection(QueryString);
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

        //Finds Security groups that the user belongs to
        public void SetSecurityGroups()
        {
            bool memberTrue = false;
            string secname = "";
            string secID = "";
            string secPath = "";
            string projectID = "";

            foreach (Project p in CheckedUser.UserProjects)
            {
                if (p.IsMemberOf == true)
                {
                    projectID = p.ProjectID;
                    break;
                }
            }
            SqlConnection PMNConnection = new SqlConnection(QueryString);
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

        //Sets Global/Site Wide Permissions
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
            SqlConnection PMNConnection = new SqlConnection(QueryString);
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
                if (name != prevname && prevname != "")
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

        //This is the meat and bones of the program. It auto generates all of the nonglobal tables using reflection.
        public void AutoGenerateTables()
        {            
            List<string> Locations = new List<string>();
            List<string> LocationLabels = new List<string>();
            List<string> LocationComboBox = new List<string>();

            splitContainer1.Panel2.Controls.Add(GlobalListTitle);
            splitContainer1.Panel2.Controls.Add(GlobalPermissionsList);
            Type PermissionIden = typeof(Lpp.Dns.DTO.Security.PermissionIdentifiers);
            Type[] PermissionsLocations = PermissionIden.GetNestedTypes();
            
            foreach (Type pl in PermissionsLocations)
            {
                Locations.Add(pl.Name);
            }
            
            for (int i = 0; i < Locations.Count; i++)
            {
                Label ListBoxLabel = new Label();
                ListBoxLabel.Font = new Font("Arial Narrow", 16);
                ListBoxLabel.Text = Locations[i];
                if (Locations[i] == "ProjectRequestTypeWorkflowActivities")
                {
                    Locations[i] = "PRTWA";
                }
                LocationLabels.Add(String.Format("{0}Label", Locations[i]));
                ListBoxLabel.Name = LocationLabels[i];
                ListBoxLabel.Text = Locations[i];
                ListBoxLabel.AutoSize = true;
                if (i%2 == 1)
                {
                    ListBoxLabel.Location = new Point(21, 2 + 108 * (i + 1));
                }
                else
                {
                    ListBoxLabel.Location = new Point(492, 2 + 108 * i);
                }
                
                this.Controls.Add(ListBoxLabel);
                splitContainer1.Panel2.Controls.Add(ListBoxLabel);                
            }
           
            for (int i = 0; i < Locations.Count; i++)
            {
                CheckedListBox chklistbox = new CheckedListBox();
                chklistbox.Font = new Font("Arial Narrow", 12);
                LocationComboBox.Add(String.Format("{0}ChkListBox", Locations[i]));
                chklistbox.Name = LocationComboBox[i];
                chklistbox.Height = 151;
                chklistbox.Width = 404;
                if (i % 2 == 1)
                {
                    chklistbox.Location = new Point(22, 36 + 108 * (i + 1));
                }
                else
                {
                    chklistbox.Location = new Point(494, 36 + 108 * i);
                }
                this.Controls.Add(chklistbox);
                splitContainer1.Panel2.Controls.Add(chklistbox);
                MemberInfo[] Permissions = PermissionsLocations[i].GetMembers();
                List<string> PermissionNames = new List<string>();
                for (int j = 4; j < Permissions.Length; j++ )
                {
                    PermissionNames.Add(Permissions[j].Name);
                }                   
                chklistbox.DataSource = PermissionNames;

            }

            for (int i = 0; i < Locations.Count; i++)
            {
                ComboBox combobox = new ComboBox();
                combobox.Font = new Font("Arial Narrow", 12);
                LocationComboBox.Add(String.Format("{0}ComboBox", Locations[i]));
                combobox.Name = LocationComboBox[i];
                combobox.Height = 28;
                combobox.Width = 214;
                if (i % 2 == 1)
                {
                    combobox.Location = new Point(212, 2 + 108 * (i + 1));
                }
                else
                {
                    combobox.Location = new Point(685, 2 + 108 * i);
                }
                this.Controls.Add(combobox);
                combobox.SelectedIndexChanged += SelectedIndexChanged;
                splitContainer1.Panel2.Controls.Add(combobox);

                string name = "";
                string id = "";
                string tablename = "";
                string location = "";
                MemberList MemberDS = new MemberList();

                if (PermissionsLocations[i].Name == "Registry")
                {
                    tablename = "Registries";
                }
                else
                {
                    tablename = String.Format("{0}s", PermissionsLocations[i].Name);
                }

                SqlConnection PMNConnection = new SqlConnection(QueryString);
                PMNConnection.Open();
                SqlDataReader MemberReader = null;
                SqlCommand MemberCommand = new SqlCommand(String.Format("select * from dbo.{0}", tablename), PMNConnection);
                try
                {
                    MemberReader = MemberCommand.ExecuteReader();
                    while (MemberReader.Read())
                    {
                        name = MemberReader["Name"].ToString();
                        id = MemberReader["ID"].ToString();
                        location = PermissionsLocations[i].Name;
                        MemberDS.AddMember(name, id, location);
                    }
                    PMNConnection.Close();

                    combobox.DataSource = MemberDS.ListOfMembers;
                }
                catch (Exception)
                {
                   
                }                                
            }
        }

        //This is what checks/unchecks all of the items in the checked list boxes
        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox currentComboBox = ((ComboBox)sender);            
            string ID = "";
            string locationName = "";
            string tableName = "";
            string currentChkListBoxName = "";
            string perName = "";
            bool permissionAllowed = false;
            ID = ((Member)currentComboBox.SelectedItem).ID;
            locationName = ((Member)currentComboBox.SelectedItem).LocationName;
            if (locationName == "Registry")
            {
                tableName = "Registries";
            }
            else
            {
                tableName = String.Format("{0}s", locationName);
            }
            currentChkListBoxName = String.Format("{0}ChkListBox", locationName);
            System.Windows.Forms.CheckedListBox curentChkListBox = (System.Windows.Forms.CheckedListBox)splitContainer1.Panel2.Controls[currentChkListBoxName];


            List<GlobalPermission> Permissions = new List<GlobalPermission>();
            SqlConnection PMNConnection = new SqlConnection(QueryString);
            PMNConnection.Open();
            SqlDataReader PermissionsReader = null;
            SqlCommand PermissionsCommand = new SqlCommand(String.Format("select SecurityGroupID, PermissionID, ID, Name, {0}ID, Allowed from dbo.ACL{1} inner join dbo.Permissions on PermissionID = ID where {0}ID = '{2}'", locationName, tableName, ID), PMNConnection);
            PermissionsReader = PermissionsCommand.ExecuteReader();
            while (PermissionsReader.Read())
            {
                foreach (SecurityGroup g in CheckedUser.SecurityGroups)
                {
                    if (Permissions.Count == 0)
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
                            perName = PermissionsReader["Name"].ToString();
                            Permissions.Add(new GlobalPermission
                            {
                                GlobalPermissionName = PermissionsReader["Name"].ToString(),
                                GlobalPermissionID = PermissionsReader["PermissionID"].ToString(),
                                Allowed = permissionAllowed
                            }); 
                            break;
                        }
                    }
                    else if (Permissions.Last().GlobalPermissionID != PermissionsReader["PermissionID"].ToString())
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
                            perName = PermissionsReader["Name"].ToString();
                            Permissions.Add(new GlobalPermission
                            {
                                GlobalPermissionName = PermissionsReader["Name"].ToString(),
                                GlobalPermissionID = PermissionsReader["PermissionID"].ToString(),
                                Allowed = permissionAllowed
                            }); 
                            break;
                        }
                    }
                    else if (Permissions.Last().GlobalPermissionID == PermissionsReader["PermissionID"].ToString() && Permissions.Last().Allowed == false)
                    {
                        if (g.SecurityGroupID == PermissionsReader["SecurityGroupID"].ToString())
                        {
                            if (PermissionsReader["Allowed"].ToString() == "True" && g.IsMemberOf == true)
                            {
                                perName = PermissionsReader["Name"].ToString();
                                Permissions.Last().Allowed = true;
                            }
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                    
                }

                   for (int i = 0; i < curentChkListBox.Items.Count; i++ )
                   {
                       if (curentChkListBox.Items[i].ToString() == perName.Replace(" ", "") && permissionAllowed == true)
                       {
                           curentChkListBox.SetItemChecked(i, true);
                       }
                       else
                       {
                           curentChkListBox.SetItemChecked(i, false);
                       }
              
                   }                 
            }
            PMNConnection.Close();
        }

       

        

       

                  
    }
}
       
