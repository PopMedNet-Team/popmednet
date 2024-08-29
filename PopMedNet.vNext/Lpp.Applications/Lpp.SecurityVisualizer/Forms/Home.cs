using Dapper;
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

namespace Lpp.SecurityVisualizer.Forms
{
    public partial class Home : Form
    {
        bool dbTested = false;
        ProgressBar _testBar = new ProgressBar();
        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, System.EventArgs e)
        {
#if DEBUG
            txtServer.Text = "localhost";
            txtDbName.Text = "pmn_trunk";
            txtDbUserName.Text = "blah";
            txtDbPassword.Text = "Boat45205!$";
#endif
        }

        private void txtServer_KeyUp(object sender, EventArgs e)
        {
            CheckToDisablePermissionTypeDropdown();
        }

        private void txtDbName_KeyUp(object sender, EventArgs e)
        {
            CheckToDisablePermissionTypeDropdown();
        }

        private void txtDbUserName_KeyUp(object sender, EventArgs e)
        {
            CheckToDisablePermissionTypeDropdown();
        }

        private void txtDbPassword_KeyUp(object sender, EventArgs e)
        {
            CheckToDisablePermissionTypeDropdown();
        }

        private void cboPermissionType_EnabledChanged(object sender, EventArgs e)
        {
            if (cboPermissionType.Enabled == false)
            {
                cboPermissions.Enabled = false;
                cboUsers.Enabled = false;
            }
        }

        private void cboPermissionType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboPermissionType.SelectedValue is int)
            {
                if ((int)cboPermissionType.SelectedValue == 1)
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                    builder.DataSource = txtServer.Text;
                    builder.UserID = txtDbUserName.Text;
                    builder.Password = txtDbPassword.Text;
                    builder.InitialCatalog = txtDbName.Text;
                    builder.IntegratedSecurity = false;
                    using (var connection = new SqlConnection(builder.ConnectionString))
                    {
                        var permissions = connection.Query<Models.PermissionsAndEvents>("select ID, Name from Permissions ORDER BY Name").ToList();

                        BindingSource permissionsList = new BindingSource();

                        permissionsList.DataSource = permissions;

                        cboPermissions.DataSource = permissionsList;
                        cboPermissions.DisplayMember = "Name";
                        cboPermissions.ValueMember = "ID";
                    }
                    cboPermissions.Enabled = true;
                    btnSearchPermissions.Enabled = true;
                    btnReset.Enabled = true;
                }
                else if ((int)cboPermissionType.SelectedValue == 2)
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                    builder.DataSource = txtServer.Text;
                    builder.UserID = txtDbUserName.Text;
                    builder.Password = txtDbPassword.Text;
                    builder.InitialCatalog = txtDbName.Text;
                    builder.IntegratedSecurity = false;
                    using (var connection = new SqlConnection(builder.ConnectionString))
                    {
                        var events = connection.Query<Models.PermissionsAndEvents>("select ID, Name from Events ORDER BY Name").ToList();

                        BindingSource permissionsList = new BindingSource();

                        permissionsList.DataSource = events;

                        cboPermissions.DataSource = permissionsList;
                        cboPermissions.DisplayMember = "Name";
                        cboPermissions.ValueMember = "ID";
                    }
                    cboPermissions.Enabled = true;
                    btnSearchPermissions.Enabled = true;
                    btnReset.Enabled = true;
                }
                else
                {
                    cboPermissions.Enabled = false;
                    btnSearchPermissions.Enabled = false;
                    btnReset.Enabled = false;
                }
            }
        }

        private void CheckToDisablePermissionTypeDropdown()
        {
            if (dbTested)
            {
                gridResultsNotifications.DataSource = null;
                gridResultsNotifications.Visible = false;
                gridResults.DataSource = null;
                gridResults.Visible = false;
                btnReset.Enabled = false;
                btnResetNotifcations.Enabled = false;
                btnSearchPermissions.Enabled = false;
                btnSearchNotifications.Enabled = false;
                if (cboPermissions.Enabled)
                {
                    cboPermissions.SelectedIndex = 0;
                }
                if (cboUsers.Enabled)
                {
                    cboUsers.SelectedIndex = 0;
                }
                if (cboUsersForNotifications.Enabled)
                {
                    cboUsersForNotifications.SelectedIndex = 0;
                }
                if (cboPermissionType.Enabled)
                {
                    cboPermissionType.SelectedIndex = 0;
                }
                cboPermissions.Enabled = false;
                cboUsers.Enabled = false;
                cboUsersForNotifications.Enabled = false;
                cboPermissionType.Enabled = false;
                dbTested = false;
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = txtServer.Text;
            builder.UserID = txtDbUserName.Text;
            builder.Password = txtDbPassword.Text;
            builder.InitialCatalog = txtDbName.Text;
            builder.IntegratedSecurity = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    connection.Close();
                    dbTested = true;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                dbTested = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbTested = false;
            }
            finally
            {
                if (dbTested)
                {
                    List<KeyValuePair<int, string>> dslist = new List<KeyValuePair<int, string>>();
                    dslist.Add(new KeyValuePair<int, string>(0, "Select One"));
                    dslist.Add(new KeyValuePair<int, string>(1, "Permissions"));
                    dslist.Add(new KeyValuePair<int, string>(2, "Events"));
                    BindingSource bslist = new BindingSource();

                    bslist.DataSource = dslist;

                    cboPermissionType.DataSource = bslist;
                    cboPermissionType.DisplayMember = "Value";
                    cboPermissionType.ValueMember = "Key";

                    cboPermissionType.Enabled = true;
                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                    {
                        connection.Open();

                        var users = connection.Query<Models.User>("select ID, UserName from Users WHERE isDeleted = 0 ORDER BY UserName").ToList();

                        BindingSource usersList = new BindingSource();

                        usersList.DataSource = users;

                        cboUsers.DataSource = usersList;
                        cboUsers.DisplayMember = "UserName";
                        cboUsers.ValueMember = "ID";
                        cboUsers.Enabled = true;

                        cboUsersForNotifications.DataSource = usersList;
                        cboUsersForNotifications.DisplayMember = "UserName";
                        cboUsersForNotifications.ValueMember = "ID";
                        cboUsersForNotifications.Enabled = true;

                        btnSearchNotifications.Enabled = true;
                        btnResetNotifcations.Enabled = true;
                    }
                }
            }
        }

        private void btnSearchPermissions_Click(object sender, EventArgs e)
        {
            if ((int)cboPermissionType.SelectedValue == 1)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = txtServer.Text;
                builder.UserID = txtDbUserName.Text;
                builder.Password = txtDbPassword.Text;
                builder.InitialCatalog = txtDbName.Text;
                builder.IntegratedSecurity = false;
                using (var connection = new SqlConnection(builder.ConnectionString))
                {
                    var securityGroups = DataAccess.GetSecurityGroups(connection, cboUsers.SelectedValue);
                    var permissionID = Guid.Parse(cboPermissions.SelectedValue.ToString());

                    DataTable table = new DataTable();
                    table.Columns.Add("Security Group Type", typeof(string));
                    table.Columns.Add("Security Group", typeof(string));
                    table.Columns.Add("Parent Security Group", typeof(string));
                    table.Columns.Add("Location", typeof(string));
                    table.Columns.Add("Additional Info", typeof(string));
                    table.Columns.Add("Allowed/Denied", typeof(string));

                    foreach (var sg in securityGroups)
                    {
                        var globalRes = DataAccess.Permissions.GetGlobalPermissions(connection, sg.ID, permissionID);
                        var dmRes = DataAccess.Permissions.GetDataMartPermissions(connection, sg.ID, permissionID);
                        var groupRes = DataAccess.Permissions.GetGroupsPermissions(connection, sg.ID, permissionID);
                        var orgRes = DataAccess.Permissions.GetOrganizationPermissions(connection, sg.ID, permissionID);
                        var orgDmRes = DataAccess.Permissions.GetOrganizationDataMartPermissions(connection, sg.ID, permissionID);
                        var orgUserRes = DataAccess.Permissions.GetOrganizationUserPermissions(connection, sg.ID, permissionID);
                        var projRes = DataAccess.Permissions.GetProjectPermissions(connection, sg.ID, permissionID);
                        var projDMRes = DataAccess.Permissions.GetProjectDataMartPermissions(connection, sg.ID, permissionID);
                        var projOrgRes = DataAccess.Permissions.GetProjectOrganizationPermissions(connection, sg.ID, permissionID);
                        //var projReqTypeRes = DataAccess.Permissions.GetProjectRequestTypesPermissions(connection, sg.ID, permissionID);
                        var projReqTypeActRes = DataAccess.Permissions.GetProjectRequestTypesWorkflowActivityPermissions(connection, sg.ID, permissionID);
                        var regRes = DataAccess.Permissions.GetRegistryPermissions(connection, sg.ID, permissionID);
                        var reqRes = DataAccess.Permissions.GetRequestPermissions(connection, sg.ID, permissionID);
                        var reqTypeRes = DataAccess.Permissions.GetRequestTypePermissions(connection, sg.ID, permissionID);
                        var tempRes = DataAccess.Permissions.GetTemplatesPermissions(connection, sg.ID, permissionID);
                        var usrRes = DataAccess.Permissions.GetUserPermissions(connection, sg.ID, permissionID);

                        foreach (var res in globalRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Global", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Permissions.GetGlobalPermissions(connection, parentID.Value, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Global", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in dmRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Datamart", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Permissions.GetDataMartPermissions(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "DataMarts", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in groupRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Groups", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Permissions.GetGroupsPermissions(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Groups", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in orgRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Organizations", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Permissions.GetOrganizationPermissions(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Organizations", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in orgDmRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Organization DataMart", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Permissions.GetOrganizationDataMartPermissions(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Organization DataMart", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in orgUserRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Organization User", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Permissions.GetOrganizationUserPermissions(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Organization User", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in projRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Project", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Permissions.GetProjectPermissions(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Project", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in projReqTypeActRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Project Request Type Activities", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Permissions.GetProjectRequestTypesWorkflowActivityPermissions(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Project Request Type Activities", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in projDMRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Project DataMart", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Permissions.GetProjectDataMartPermissions(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Project DataMart", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in projOrgRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Project Organization", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Permissions.GetProjectOrganizationPermissions(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Project Organization", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in regRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Registries", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Permissions.GetRegistryPermissions(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Registries", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in reqRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Requests", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Permissions.GetRequestPermissions(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Requests", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in reqTypeRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Request Types", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Permissions.GetRequestTypePermissions(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Request Types", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in tempRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Templates", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Permissions.GetTemplatesPermissions(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Templates", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in usrRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Users", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Permissions.GetUserPermissions(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Users", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                    }

                    gridResults.DataSource = table;
                    gridResults.Visible = true;
                    gridResults.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    gridResults.Columns[0].Width = 200;
                    gridResults.Columns[1].Width = 200;
                    gridResults.Columns[2].Width = 200;
                    gridResults.Columns[3].Width = 200;
                    gridResults.Columns[4].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    gridResults.Columns[4].Width = 500;
                    gridResults.Columns[5].Width = 200;
                }
            }
            else if ((int)cboPermissionType.SelectedValue == 2)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = txtServer.Text;
                builder.UserID = txtDbUserName.Text;
                builder.Password = txtDbPassword.Text;
                builder.InitialCatalog = txtDbName.Text;
                builder.IntegratedSecurity = false;
                using (var connection = new SqlConnection(builder.ConnectionString))
                {
                    var securityGroups = DataAccess.GetSecurityGroups(connection, cboUsers.SelectedValue);
                    var permissionID = Guid.Parse(cboPermissions.SelectedValue.ToString());

                    DataTable table = new DataTable();
                    table.Columns.Add("Security Group Type", typeof(string));
                    table.Columns.Add("Security Group", typeof(string));
                    table.Columns.Add("Parent Security Group", typeof(string));
                    table.Columns.Add("Location", typeof(string));
                    table.Columns.Add("Additional Info", typeof(string));
                    table.Columns.Add("Allowed/Denied", typeof(string));

                    foreach (var sg in securityGroups)
                    {
                        var globalRes = DataAccess.Events.GetGlobalEvents(connection, sg.ID, permissionID);
                        var dmRes = DataAccess.Events.GetDataMartEvents(connection, sg.ID, permissionID);
                        var groupRes = DataAccess.Events.GetGroupsEvents(connection, sg.ID, permissionID);
                        var orgRes = DataAccess.Events.GetOrganizationEvents(connection, sg.ID, permissionID);
                        var projRes = DataAccess.Events.GetProjectEvents(connection, sg.ID, permissionID);
                        var projDMRes = DataAccess.Events.GetProjectDataMartEvents(connection, sg.ID, permissionID);
                        var projOrgRes = DataAccess.Events.GetProjectOrganizationEvents(connection, sg.ID, permissionID);
                        var regRes = DataAccess.Events.GetRegistryEvents(connection, sg.ID, permissionID);
                        var usrRes = DataAccess.Events.GetUserEvents(connection, sg.ID, permissionID);

                        foreach (var res in globalRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Global", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Events.GetGlobalEvents(connection, parentID.Value, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Global", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in dmRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Datamart", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Events.GetDataMartEvents(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "DataMarts", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in groupRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Groups", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Events.GetGroupsEvents(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Groups", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in orgRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Organizations", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Events.GetOrganizationEvents(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Organizations", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in projRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Project", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Events.GetProjectEvents(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Project", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in projDMRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                            }
                            
                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Project DataMart", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Events.GetProjectDataMartEvents(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Project DataMart", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in projOrgRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);

                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Project Organization", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Events.GetProjectOrganizationEvents(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Project Organization", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in regRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);

                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Registries", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Events.GetRegistryEvents(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Registries", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                        foreach (var res in usrRes)
                        {
                            var parentName = "";

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = sg.ParentSecurityGroupID.Value });
                                parentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);

                            }

                            table.Rows.Add(sg.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", sg.Owner, sg.Name), parentName, "Users", res.AdditionalInfo, res.Allowed ? "Allowed" : "Denied");

                            if (sg.ParentSecurityGroupID.HasValue)
                            {
                                Guid? parentID = sg.ParentSecurityGroupID.Value;

                                while (parentID.HasValue)
                                {
                                    var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                    var parentParentName = "";
                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        var parent = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentSG.ParentSecurityGroupID.Value });
                                        parentParentName = string.Format("{0}\\{1}", parent.Owner, parent.Name);
                                    }

                                    var parentGlobalRes = DataAccess.Events.GetUserEvents(connection, parentSG.ID, permissionID);
                                    foreach (var parentRes in parentGlobalRes)
                                    {
                                        table.Rows.Add(parentSG.Type == 1 ? "Organization" : "Project", string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name), parentParentName, "Users", parentRes.AdditionalInfo, parentRes.Allowed ? "Allowed" : "Denied");
                                    }

                                    if (parentSG.ParentSecurityGroupID.HasValue)
                                    {
                                        parentID = parentSG.ParentSecurityGroupID.Value;
                                    }
                                    else
                                    {
                                        parentID = null;
                                    }
                                }
                            }
                        }
                    }

                    gridResults.DataSource = table;
                    gridResults.Visible = true;
                    gridResults.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    gridResults.Columns[0].Width = 200;
                    gridResults.Columns[1].Width = 200;
                    gridResults.Columns[2].Width = 200;
                    gridResults.Columns[3].Width = 200;
                    gridResults.Columns[4].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    gridResults.Columns[4].Width = 500;
                    gridResults.Columns[5].Width = 200;
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            gridResultsNotifications.DataSource = null;
            gridResultsNotifications.Visible = false;
            gridResults.DataSource = null;
            gridResults.Visible = false;
            btnReset.Enabled = false;
            btnSearchPermissions.Enabled = false;
            if(cboPermissions.Enabled)
                cboPermissions.SelectedIndex = 0;
            cboUsers.SelectedIndex = 0;
            if(cboPermissionType.Enabled)
                cboPermissionType.SelectedIndex = 0;
            cboPermissions.Enabled = false;
        }

        private void btnSearchNotifications_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = txtServer.Text;
            builder.UserID = txtDbUserName.Text;
            builder.Password = txtDbPassword.Text;
            builder.InitialCatalog = txtDbName.Text;
            builder.IntegratedSecurity = false;
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                DataTable table = new DataTable();
                table.Columns.Add("Event", typeof(string));
                table.Columns.Add("Notification Setting", typeof(string));
                table.Columns.Add("Notification For My Setting", typeof(string));
                table.Columns.Add("Allowed", typeof(string));
                table.Columns.Add("Denied By", typeof(string));

                var results = DataAccess.GetUserSubscribedNotifications(connection, cboUsersForNotifications.SelectedValue);

                foreach (var res in results)
                {

                    var securityGroups = DataAccess.GetSecurityGroups(connection, cboUsersForNotifications.SelectedValue);

                    var allowed = true;
                    var deniedOn = "";

                    foreach (var sg in securityGroups)
                    {
                        var pers = DataAccess.Events.GetDataMartEvents(connection, sg.ID, res.EventID).Concat(
                            DataAccess.Events.GetGlobalEvents(connection, sg.ID, res.EventID)).Concat(
                            DataAccess.Events.GetGroupsEvents(connection, sg.ID, res.EventID)).Concat(
                            DataAccess.Events.GetOrganizationEvents(connection, sg.ID, res.EventID)).Concat(
                            DataAccess.Events.GetProjectDataMartEvents(connection, sg.ID, res.EventID)).Concat(
                            DataAccess.Events.GetProjectEvents(connection, sg.ID, res.EventID)).Concat(
                            DataAccess.Events.GetProjectOrganizationEvents(connection, sg.ID, res.EventID)).Concat(
                            DataAccess.Events.GetRegistryEvents(connection, sg.ID, res.EventID)).Concat(
                            DataAccess.Events.GetUserEvents(connection, sg.ID, res.EventID));

                        if(pers.Any(x => x.Allowed == false))
                        {
                            deniedOn = string.Format("{0}\\{1}", sg.Owner, sg.Name);
                            allowed = false;
                            break;
                        }

                        if (sg.ParentSecurityGroupID.HasValue)
                        {
                            Guid? parentID = sg.ParentSecurityGroupID.Value;

                            while (parentID.HasValue)
                            {
                                var parentSG = connection.QueryFirstOrDefault<Models.SecurityGroup>(@"Select * from SecurityGroups WHERE ID = @ID", new { ID = parentID.Value });

                                var parentPers = DataAccess.Events.GetDataMartEvents(connection, parentSG.ID, res.EventID).Concat(
                                           DataAccess.Events.GetGlobalEvents(connection, parentSG.ID, res.EventID)).Concat(
                                           DataAccess.Events.GetGroupsEvents(connection, parentSG.ID, res.EventID)).Concat(
                                           DataAccess.Events.GetOrganizationEvents(connection, parentSG.ID, res.EventID)).Concat(
                                           DataAccess.Events.GetProjectDataMartEvents(connection, parentSG.ID, res.EventID)).Concat(
                                           DataAccess.Events.GetProjectEvents(connection, parentSG.ID, res.EventID)).Concat(
                                           DataAccess.Events.GetProjectOrganizationEvents(connection, parentSG.ID, res.EventID)).Concat(
                                           DataAccess.Events.GetRegistryEvents(connection, parentSG.ID, res.EventID)).Concat(
                                           DataAccess.Events.GetUserEvents(connection, parentSG.ID, res.EventID));

                                if (parentPers.Any(x => x.Allowed == false))
                                {
                                    deniedOn = string.Format("{0}\\{1}", parentSG.Owner, parentSG.Name);
                                    allowed = false;
                                    break;
                                }
                            }
                        }
                    }


                    var freq = "";

                    if (res.Frequency == 0)
                        freq = "Immediately";
                    else if (res.Frequency == 1)
                        freq = "Daily";
                    else if (res.Frequency == 2)
                        freq = "Weekly";
                    else if (res.Frequency == 3)
                        freq = "Monthly";

                    var myFreq = "";

                    if (res.FrequencyForMy.HasValue)
                    {
                        if (res.FrequencyForMy.Value == 0)
                            myFreq = "Immediately";
                        else if (res.FrequencyForMy.Value == 1)
                            myFreq = "Daily";
                        else if (res.FrequencyForMy.Value == 2)
                            myFreq = "Weekly";
                        else if (res.FrequencyForMy.Value == 3)
                            myFreq = "Monthly";
                    }
                    

                    table.Rows.Add(res.EventName, freq, myFreq, allowed ? "Allowed" : "Denied", deniedOn);
                }


                gridResultsNotifications.DataSource = table;
                gridResultsNotifications.Visible = true;
                gridResultsNotifications.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
        }
    }
}
