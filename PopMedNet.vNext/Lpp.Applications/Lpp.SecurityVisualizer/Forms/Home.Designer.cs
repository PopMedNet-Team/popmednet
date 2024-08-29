namespace Lpp.SecurityVisualizer.Forms
{
    partial class Home
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblServer = new System.Windows.Forms.Label();
            this.lblDatabaseName = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtDbName = new System.Windows.Forms.TextBox();
            this.txtDbPassword = new System.Windows.Forms.TextBox();
            this.txtDbUserName = new System.Windows.Forms.TextBox();
            this.lblDatabasePassword = new System.Windows.Forms.Label();
            this.lblDatabaseUser = new System.Windows.Forms.Label();
            this.lblPerType = new System.Windows.Forms.Label();
            this.cboPermissionType = new System.Windows.Forms.ComboBox();
            this.cboPermissions = new System.Windows.Forms.ComboBox();
            this.lblPermission = new System.Windows.Forms.Label();
            this.gridResults = new System.Windows.Forms.DataGridView();
            this.btnTest = new System.Windows.Forms.Button();
            this.cboUsers = new System.Windows.Forms.ComboBox();
            this.lblUsers = new System.Windows.Forms.Label();
            this.btnSearchPermissions = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.lblUsersNotification = new System.Windows.Forms.Label();
            this.cboUsersForNotifications = new System.Windows.Forms.ComboBox();
            this.btnSearchNotifications = new System.Windows.Forms.Button();
            this.btnResetNotifcations = new System.Windows.Forms.Button();
            this.gridResultsNotifications = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridResults)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResultsNotifications)).BeginInit();
            this.SuspendLayout();
            // 
            // lblServer
            // 
            this.lblServer.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(37, 9);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(41, 13);
            this.lblServer.TabIndex = 0;
            this.lblServer.Text = "Server:";
            // 
            // lblDatabaseName
            // 
            this.lblDatabaseName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDatabaseName.AutoSize = true;
            this.lblDatabaseName.Location = new System.Drawing.Point(441, 9);
            this.lblDatabaseName.Name = "lblDatabaseName";
            this.lblDatabaseName.Size = new System.Drawing.Size(87, 13);
            this.lblDatabaseName.TabIndex = 1;
            this.lblDatabaseName.Text = "Database Name:";
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Location = new System.Drawing.Point(84, 5);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(308, 20);
            this.txtServer.TabIndex = 4;
            this.txtServer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtServer_KeyUp);
            // 
            // txtDbName
            // 
            this.txtDbName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDbName.Location = new System.Drawing.Point(534, 5);
            this.txtDbName.Name = "txtDbName";
            this.txtDbName.Size = new System.Drawing.Size(263, 20);
            this.txtDbName.TabIndex = 5;
            this.txtDbName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtDbName_KeyUp);
            // 
            // txtDbPassword
            // 
            this.txtDbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDbPassword.Location = new System.Drawing.Point(534, 35);
            this.txtDbPassword.Name = "txtDbPassword";
            this.txtDbPassword.Size = new System.Drawing.Size(263, 20);
            this.txtDbPassword.TabIndex = 9;
            this.txtDbPassword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtDbPassword_KeyUp);
            // 
            // txtDbUserName
            // 
            this.txtDbUserName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDbUserName.Location = new System.Drawing.Point(84, 35);
            this.txtDbUserName.Name = "txtDbUserName";
            this.txtDbUserName.Size = new System.Drawing.Size(308, 20);
            this.txtDbUserName.TabIndex = 8;
            this.txtDbUserName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtDbUserName_KeyUp);
            // 
            // lblDatabasePassword
            // 
            this.lblDatabasePassword.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDatabasePassword.AutoSize = true;
            this.lblDatabasePassword.Location = new System.Drawing.Point(398, 39);
            this.lblDatabasePassword.Name = "lblDatabasePassword";
            this.lblDatabasePassword.Size = new System.Drawing.Size(130, 13);
            this.lblDatabasePassword.TabIndex = 7;
            this.lblDatabasePassword.Text = "Database User Password:";
            // 
            // lblDatabaseUser
            // 
            this.lblDatabaseUser.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDatabaseUser.AutoSize = true;
            this.lblDatabaseUser.Location = new System.Drawing.Point(3, 39);
            this.lblDatabaseUser.Name = "lblDatabaseUser";
            this.lblDatabaseUser.Size = new System.Drawing.Size(75, 13);
            this.lblDatabaseUser.TabIndex = 6;
            this.lblDatabaseUser.Text = "Databse User:";
            // 
            // lblPerType
            // 
            this.lblPerType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPerType.AutoSize = true;
            this.lblPerType.Location = new System.Drawing.Point(3, 7);
            this.lblPerType.Name = "lblPerType";
            this.lblPerType.Size = new System.Drawing.Size(80, 13);
            this.lblPerType.TabIndex = 10;
            this.lblPerType.Text = "Query Type:";
            // 
            // cboPermissionType
            // 
            this.cboPermissionType.Enabled = false;
            this.cboPermissionType.FormattingEnabled = true;
            this.cboPermissionType.Location = new System.Drawing.Point(89, 3);
            this.cboPermissionType.Name = "cboPermissionType";
            this.cboPermissionType.Size = new System.Drawing.Size(166, 21);
            this.cboPermissionType.TabIndex = 11;
            this.cboPermissionType.SelectedValueChanged += new System.EventHandler(this.cboPermissionType_SelectedValueChanged);
            this.cboPermissionType.EnabledChanged += new System.EventHandler(this.cboPermissionType_EnabledChanged);
            // 
            // cboPermissions
            // 
            this.cboPermissions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboPermissions.Enabled = false;
            this.cboPermissions.FormattingEnabled = true;
            this.cboPermissions.Location = new System.Drawing.Point(332, 3);
            this.cboPermissions.Name = "cboPermissions";
            this.cboPermissions.Size = new System.Drawing.Size(180, 21);
            this.cboPermissions.TabIndex = 13;
            // 
            // lblPermission
            // 
            this.lblPermission.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPermission.AutoSize = true;
            this.lblPermission.Location = new System.Drawing.Point(261, 7);
            this.lblPermission.Name = "lblPermission";
            this.lblPermission.Size = new System.Drawing.Size(65, 13);
            this.lblPermission.TabIndex = 12;
            this.lblPermission.Text = "Permission:";
            // 
            // gridResults
            // 
            this.gridResults.AllowUserToAddRows = false;
            this.gridResults.AllowUserToDeleteRows = false;
            this.gridResults.AllowUserToOrderColumns = true;
            this.gridResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridResults.Location = new System.Drawing.Point(3, 69);
            this.gridResults.Name = "gridResults";
            this.gridResults.ReadOnly = true;
            this.gridResults.Size = new System.Drawing.Size(786, 478);
            this.gridResults.TabIndex = 14;
            this.gridResults.Visible = false;
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(3, 12);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(794, 23);
            this.btnTest.TabIndex = 15;
            this.btnTest.Text = "Test Database Connection";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // cboUsers
            // 
            this.cboUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboUsers.Enabled = false;
            this.cboUsers.FormattingEnabled = true;
            this.cboUsers.Location = new System.Drawing.Point(565, 3);
            this.cboUsers.Name = "cboUsers";
            this.cboUsers.Size = new System.Drawing.Size(218, 21);
            this.cboUsers.TabIndex = 16;
            // 
            // lblUsers
            // 
            this.lblUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsers.AutoSize = true;
            this.lblUsers.Location = new System.Drawing.Point(518, 7);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(41, 13);
            this.lblUsers.TabIndex = 17;
            this.lblUsers.Text = "Users:";
            // 
            // btnSearchPermissions
            // 
            this.btnSearchPermissions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchPermissions.Enabled = false;
            this.btnSearchPermissions.Location = new System.Drawing.Point(3, 3);
            this.btnSearchPermissions.Name = "btnSearchPermissions";
            this.btnSearchPermissions.Size = new System.Drawing.Size(387, 23);
            this.btnSearchPermissions.TabIndex = 18;
            this.btnSearchPermissions.Text = "Search";
            this.btnSearchPermissions.UseVisualStyleBackColor = true;
            this.btnSearchPermissions.Click += new System.EventHandler(this.btnSearchPermissions_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Enabled = false;
            this.btnReset.Location = new System.Drawing.Point(396, 3);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(387, 23);
            this.btnReset.TabIndex = 19;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(13, 364);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(775, 285);
            this.panel2.TabIndex = 21;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.txtDbPassword, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblServer, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtDbName, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblDatabaseName, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtServer, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtDbUserName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDatabaseUser, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDatabasePassword, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53.22581F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.77419F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 60);
            this.tableLayoutPanel1.TabIndex = 22;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnTest, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 60);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(800, 47);
            this.tableLayoutPanel2.TabIndex = 23;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 107);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 576);
            this.tabControl1.TabIndex = 24;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gridResults);
            this.tabPage1.Controls.Add(this.tableLayoutPanel4);
            this.tabPage1.Controls.Add(this.tableLayoutPanel3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 550);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Permissions and Events";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.btnSearchPermissions, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnReset, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 31);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(786, 38);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 6;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.12738F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.10835F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.224011F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.8653F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.130268F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.35249F));
            this.tableLayoutPanel3.Controls.Add(this.lblPerType, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.cboUsers, 5, 0);
            this.tableLayoutPanel3.Controls.Add(this.cboPermissionType, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblUsers, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblPermission, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.cboPermissions, 3, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(786, 28);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gridResultsNotifications);
            this.tabPage2.Controls.Add(this.tableLayoutPanel5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 550);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "User Notifications";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.615776F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.257F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.Controls.Add(this.lblUsersNotification, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.cboUsersForNotifications, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnSearchNotifications, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnResetNotifcations, 3, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(786, 57);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // lblUsersNotification
            // 
            this.lblUsersNotification.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsersNotification.AutoSize = true;
            this.lblUsersNotification.Location = new System.Drawing.Point(3, 22);
            this.lblUsersNotification.Name = "lblUsersNotification";
            this.lblUsersNotification.Size = new System.Drawing.Size(46, 13);
            this.lblUsersNotification.TabIndex = 0;
            this.lblUsersNotification.Text = "Users:";
            // 
            // cboUsersForNotifications
            // 
            this.cboUsersForNotifications.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboUsersForNotifications.Enabled = false;
            this.cboUsersForNotifications.FormattingEnabled = true;
            this.cboUsersForNotifications.Location = new System.Drawing.Point(55, 18);
            this.cboUsersForNotifications.Name = "cboUsersForNotifications";
            this.cboUsersForNotifications.Size = new System.Drawing.Size(334, 21);
            this.cboUsersForNotifications.TabIndex = 1;
            // 
            // btnSearchNotifications
            // 
            this.btnSearchNotifications.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchNotifications.Enabled = false;
            this.btnSearchNotifications.Location = new System.Drawing.Point(395, 17);
            this.btnSearchNotifications.Name = "btnSearchNotifications";
            this.btnSearchNotifications.Size = new System.Drawing.Size(190, 23);
            this.btnSearchNotifications.TabIndex = 2;
            this.btnSearchNotifications.Text = "Search";
            this.btnSearchNotifications.UseVisualStyleBackColor = true;
            this.btnSearchNotifications.Click += new System.EventHandler(this.btnSearchNotifications_Click);
            // 
            // btnResetNotifcations
            // 
            this.btnResetNotifcations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetNotifcations.Enabled = false;
            this.btnResetNotifcations.Location = new System.Drawing.Point(591, 17);
            this.btnResetNotifcations.Name = "btnResetNotifcations";
            this.btnResetNotifcations.Size = new System.Drawing.Size(192, 23);
            this.btnResetNotifcations.TabIndex = 3;
            this.btnResetNotifcations.Text = "Reset";
            this.btnResetNotifcations.UseVisualStyleBackColor = true;
            this.btnResetNotifcations.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // gridResultsNotifications
            // 
            this.gridResultsNotifications.AllowUserToAddRows = false;
            this.gridResultsNotifications.AllowUserToDeleteRows = false;
            this.gridResultsNotifications.AllowUserToOrderColumns = true;
            this.gridResultsNotifications.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResultsNotifications.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridResultsNotifications.Location = new System.Drawing.Point(3, 60);
            this.gridResultsNotifications.Name = "gridResultsNotifications";
            this.gridResultsNotifications.ReadOnly = true;
            this.gridResultsNotifications.Size = new System.Drawing.Size(786, 487);
            this.gridResultsNotifications.TabIndex = 2;
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 680);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel2);
            this.Name = "Home";
            this.Text = "Home";
            this.Load += new System.EventHandler(this.Home_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridResults)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResultsNotifications)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblDatabaseName;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtDbName;
        private System.Windows.Forms.TextBox txtDbPassword;
        private System.Windows.Forms.TextBox txtDbUserName;
        private System.Windows.Forms.Label lblDatabasePassword;
        private System.Windows.Forms.Label lblDatabaseUser;
        private System.Windows.Forms.Label lblPerType;
        private System.Windows.Forms.ComboBox cboPermissionType;
        private System.Windows.Forms.ComboBox cboPermissions;
        private System.Windows.Forms.Label lblPermission;
        private System.Windows.Forms.DataGridView gridResults;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.ComboBox cboUsers;
        private System.Windows.Forms.Label lblUsers;
        private System.Windows.Forms.Button btnSearchPermissions;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label lblUsersNotification;
        private System.Windows.Forms.ComboBox cboUsersForNotifications;
        private System.Windows.Forms.Button btnSearchNotifications;
        private System.Windows.Forms.Button btnResetNotifcations;
        private System.Windows.Forms.DataGridView gridResultsNotifications;
    }
}