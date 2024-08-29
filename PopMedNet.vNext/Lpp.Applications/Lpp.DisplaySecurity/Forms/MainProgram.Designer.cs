namespace Lpp.DisplaySecurity.Forms
{
    partial class MainProgram
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Settings = new System.Windows.Forms.ToolStripDropDownButton();
            this.ChangeConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.UserInputBox = new System.Windows.Forms.TextBox();
            this.UserLabel = new System.Windows.Forms.Label();
            this.UserFindBtn = new System.Windows.Forms.Button();
            this.OrganizationLabel = new System.Windows.Forms.Label();
            this.OrgInputBox = new System.Windows.Forms.TextBox();
            this.OrgFindBtn = new System.Windows.Forms.Button();
            this.ProjectList = new System.Windows.Forms.CheckedListBox();
            this.ProjectListTitle = new System.Windows.Forms.Label();
            this.SecurityGroupList = new System.Windows.Forms.CheckedListBox();
            this.SecGroupListTitle = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SCUBtn = new System.Windows.Forms.Button();
            this.FoundUsersLabel = new System.Windows.Forms.Label();
            this.UserList = new System.Windows.Forms.CheckedListBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.UserDropDown = new System.Windows.Forms.ComboBox();
            this.UserPermissionsLabel = new System.Windows.Forms.Label();
            this.UserPermissionsList = new System.Windows.Forms.CheckedListBox();
            this.DataMartDropDown = new System.Windows.Forms.ComboBox();
            this.DataMartLabel = new System.Windows.Forms.Label();
            this.DataMartPermissionsList = new System.Windows.Forms.CheckedListBox();
            this.RegistryListDropDown = new System.Windows.Forms.ComboBox();
            this.RegistryListLabel = new System.Windows.Forms.Label();
            this.RegistryPermissionsList = new System.Windows.Forms.CheckedListBox();
            this.OrgListDropDown = new System.Windows.Forms.ComboBox();
            this.OrgListTitle = new System.Windows.Forms.Label();
            this.OrgPermissionList = new System.Windows.Forms.CheckedListBox();
            this.GlobalPermissionsList = new System.Windows.Forms.CheckedListBox();
            this.GlobalListTitle = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.Settings});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(946, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // FileMenu
            // 
            this.FileMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.FileMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(38, 22);
            this.FileMenu.Text = "File";
            this.FileMenu.ToolTipText = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Settings
            // 
            this.Settings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Settings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChangeConnection});
            this.Settings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(62, 22);
            this.Settings.Text = "Settings";
            this.Settings.ToolTipText = "Settings";
            // 
            // ChangeConnection
            // 
            this.ChangeConnection.Name = "ChangeConnection";
            this.ChangeConnection.Size = new System.Drawing.Size(180, 22);
            this.ChangeConnection.Text = "Change Connection";
            this.ChangeConnection.Click += new System.EventHandler(this.ChangeConnection_Click);
            // 
            // UserInputBox
            // 
            this.UserInputBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserInputBox.Location = new System.Drawing.Point(154, 15);
            this.UserInputBox.Name = "UserInputBox";
            this.UserInputBox.Size = new System.Drawing.Size(227, 22);
            this.UserInputBox.TabIndex = 1;
            // 
            // UserLabel
            // 
            this.UserLabel.AutoSize = true;
            this.UserLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserLabel.Location = new System.Drawing.Point(22, 15);
            this.UserLabel.Name = "UserLabel";
            this.UserLabel.Size = new System.Drawing.Size(76, 20);
            this.UserLabel.TabIndex = 2;
            this.UserLabel.Text = "User Name";
            // 
            // UserFindBtn
            // 
            this.UserFindBtn.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserFindBtn.Location = new System.Drawing.Point(387, 15);
            this.UserFindBtn.Name = "UserFindBtn";
            this.UserFindBtn.Size = new System.Drawing.Size(87, 22);
            this.UserFindBtn.TabIndex = 3;
            this.UserFindBtn.Text = "Find";
            this.UserFindBtn.UseVisualStyleBackColor = true;
            this.UserFindBtn.Click += new System.EventHandler(this.UserFindBtn_Click);
            // 
            // OrganizationLabel
            // 
            this.OrganizationLabel.AutoSize = true;
            this.OrganizationLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrganizationLabel.Location = new System.Drawing.Point(22, 57);
            this.OrganizationLabel.Name = "OrganizationLabel";
            this.OrganizationLabel.Size = new System.Drawing.Size(122, 20);
            this.OrganizationLabel.TabIndex = 4;
            this.OrganizationLabel.Text = "Organization Name";
            // 
            // OrgInputBox
            // 
            this.OrgInputBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrgInputBox.Location = new System.Drawing.Point(154, 57);
            this.OrgInputBox.Name = "OrgInputBox";
            this.OrgInputBox.Size = new System.Drawing.Size(227, 22);
            this.OrgInputBox.TabIndex = 5;
            // 
            // OrgFindBtn
            // 
            this.OrgFindBtn.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrgFindBtn.Location = new System.Drawing.Point(387, 57);
            this.OrgFindBtn.Name = "OrgFindBtn";
            this.OrgFindBtn.Size = new System.Drawing.Size(87, 22);
            this.OrgFindBtn.TabIndex = 6;
            this.OrgFindBtn.Text = "Find";
            this.OrgFindBtn.UseVisualStyleBackColor = true;
            this.OrgFindBtn.Click += new System.EventHandler(this.OrgFindBtn_Click);
            // 
            // ProjectList
            // 
            this.ProjectList.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProjectList.FormattingEnabled = true;
            this.ProjectList.Location = new System.Drawing.Point(26, 186);
            this.ProjectList.Name = "ProjectList";
            this.ProjectList.Size = new System.Drawing.Size(404, 151);
            this.ProjectList.TabIndex = 7;
            this.ProjectList.ThreeDCheckBoxes = true;
            // 
            // ProjectListTitle
            // 
            this.ProjectListTitle.AutoSize = true;
            this.ProjectListTitle.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProjectListTitle.Location = new System.Drawing.Point(23, 160);
            this.ProjectListTitle.Name = "ProjectListTitle";
            this.ProjectListTitle.Size = new System.Drawing.Size(76, 25);
            this.ProjectListTitle.TabIndex = 8;
            this.ProjectListTitle.Text = "Projects";
            // 
            // SecurityGroupList
            // 
            this.SecurityGroupList.CheckOnClick = true;
            this.SecurityGroupList.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SecurityGroupList.FormattingEnabled = true;
            this.SecurityGroupList.Location = new System.Drawing.Point(498, 186);
            this.SecurityGroupList.Name = "SecurityGroupList";
            this.SecurityGroupList.Size = new System.Drawing.Size(406, 151);
            this.SecurityGroupList.TabIndex = 9;
            this.SecurityGroupList.ThreeDCheckBoxes = true;
            // 
            // SecGroupListTitle
            // 
            this.SecGroupListTitle.AutoSize = true;
            this.SecGroupListTitle.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SecGroupListTitle.Location = new System.Drawing.Point(494, 160);
            this.SecGroupListTitle.Name = "SecGroupListTitle";
            this.SecGroupListTitle.Size = new System.Drawing.Size(139, 25);
            this.SecGroupListTitle.TabIndex = 10;
            this.SecGroupListTitle.Text = "Security Groups";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.SCUBtn);
            this.splitContainer1.Panel1.Controls.Add(this.FoundUsersLabel);
            this.splitContainer1.Panel1.Controls.Add(this.UserList);
            this.splitContainer1.Panel1.Controls.Add(this.SecGroupListTitle);
            this.splitContainer1.Panel1.Controls.Add(this.UserLabel);
            this.splitContainer1.Panel1.Controls.Add(this.SecurityGroupList);
            this.splitContainer1.Panel1.Controls.Add(this.UserInputBox);
            this.splitContainer1.Panel1.Controls.Add(this.OrganizationLabel);
            this.splitContainer1.Panel1.Controls.Add(this.ProjectListTitle);
            this.splitContainer1.Panel1.Controls.Add(this.OrgInputBox);
            this.splitContainer1.Panel1.Controls.Add(this.ProjectList);
            this.splitContainer1.Panel1.Controls.Add(this.UserFindBtn);
            this.splitContainer1.Panel1.Controls.Add(this.OrgFindBtn);
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.UserDropDown);
            this.splitContainer1.Panel2.Controls.Add(this.UserPermissionsLabel);
            this.splitContainer1.Panel2.Controls.Add(this.UserPermissionsList);
            this.splitContainer1.Panel2.Controls.Add(this.DataMartDropDown);
            this.splitContainer1.Panel2.Controls.Add(this.DataMartLabel);
            this.splitContainer1.Panel2.Controls.Add(this.DataMartPermissionsList);
            this.splitContainer1.Panel2.Controls.Add(this.RegistryListDropDown);
            this.splitContainer1.Panel2.Controls.Add(this.RegistryListLabel);
            this.splitContainer1.Panel2.Controls.Add(this.RegistryPermissionsList);
            this.splitContainer1.Panel2.Controls.Add(this.OrgListDropDown);
            this.splitContainer1.Panel2.Controls.Add(this.OrgListTitle);
            this.splitContainer1.Panel2.Controls.Add(this.OrgPermissionList);
            this.splitContainer1.Panel2.Controls.Add(this.GlobalPermissionsList);
            this.splitContainer1.Panel2.Controls.Add(this.GlobalListTitle);
            this.splitContainer1.Size = new System.Drawing.Size(946, 693);
            this.splitContainer1.SplitterDistance = 356;
            this.splitContainer1.TabIndex = 11;
            // 
            // SCUBtn
            // 
            this.SCUBtn.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SCUBtn.Location = new System.Drawing.Point(744, 15);
            this.SCUBtn.Name = "SCUBtn";
            this.SCUBtn.Size = new System.Drawing.Size(160, 22);
            this.SCUBtn.TabIndex = 13;
            this.SCUBtn.Text = "Select Checked Users";
            this.SCUBtn.UseVisualStyleBackColor = true;
            this.SCUBtn.Click += new System.EventHandler(this.SCUBtn_Click);
            // 
            // FoundUsersLabel
            // 
            this.FoundUsersLabel.AutoSize = true;
            this.FoundUsersLabel.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FoundUsersLabel.Location = new System.Drawing.Point(494, 15);
            this.FoundUsersLabel.Name = "FoundUsersLabel";
            this.FoundUsersLabel.Size = new System.Drawing.Size(114, 25);
            this.FoundUsersLabel.TabIndex = 12;
            this.FoundUsersLabel.Text = "Found Users";
            // 
            // UserList
            // 
            this.UserList.CheckOnClick = true;
            this.UserList.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserList.FormattingEnabled = true;
            this.UserList.Location = new System.Drawing.Point(498, 41);
            this.UserList.Name = "UserList";
            this.UserList.Size = new System.Drawing.Size(406, 109);
            this.UserList.TabIndex = 11;
            this.UserList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.UserList_ItemCheck);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(-2, -2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(918, 349);
            this.flowLayoutPanel1.TabIndex = 25;
            // 
            // UserDropDown
            // 
            this.UserDropDown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.UserDropDown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.UserDropDown.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserDropDown.FormattingEnabled = true;
            this.UserDropDown.Location = new System.Drawing.Point(117, 463);
            this.UserDropDown.Name = "UserDropDown";
            this.UserDropDown.Size = new System.Drawing.Size(327, 28);
            this.UserDropDown.TabIndex = 24;
            this.UserDropDown.SelectedIndexChanged += new System.EventHandler(this.UserDropDown_SelectedIndexChanged);
            // 
            // UserPermissionsLabel
            // 
            this.UserPermissionsLabel.AutoSize = true;
            this.UserPermissionsLabel.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserPermissionsLabel.Location = new System.Drawing.Point(22, 470);
            this.UserPermissionsLabel.Name = "UserPermissionsLabel";
            this.UserPermissionsLabel.Size = new System.Drawing.Size(49, 25);
            this.UserPermissionsLabel.TabIndex = 23;
            this.UserPermissionsLabel.Text = "User";
            // 
            // UserPermissionsList
            // 
            this.UserPermissionsList.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserPermissionsList.FormattingEnabled = true;
            this.UserPermissionsList.Location = new System.Drawing.Point(26, 496);
            this.UserPermissionsList.Name = "UserPermissionsList";
            this.UserPermissionsList.Size = new System.Drawing.Size(418, 151);
            this.UserPermissionsList.TabIndex = 22;
            this.UserPermissionsList.ThreeDCheckBoxes = true;
            // 
            // DataMartDropDown
            // 
            this.DataMartDropDown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.DataMartDropDown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.DataMartDropDown.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataMartDropDown.FormattingEnabled = true;
            this.DataMartDropDown.Location = new System.Drawing.Point(702, 240);
            this.DataMartDropDown.Name = "DataMartDropDown";
            this.DataMartDropDown.Size = new System.Drawing.Size(214, 28);
            this.DataMartDropDown.TabIndex = 21;
            this.DataMartDropDown.SelectedIndexChanged += new System.EventHandler(this.DataMartDropDown_SelectedIndexChanged);
            // 
            // DataMartLabel
            // 
            this.DataMartLabel.AutoSize = true;
            this.DataMartLabel.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataMartLabel.Location = new System.Drawing.Point(494, 247);
            this.DataMartLabel.Name = "DataMartLabel";
            this.DataMartLabel.Size = new System.Drawing.Size(85, 25);
            this.DataMartLabel.TabIndex = 20;
            this.DataMartLabel.Text = "DataMart";
            // 
            // DataMartPermissionsList
            // 
            this.DataMartPermissionsList.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataMartPermissionsList.FormattingEnabled = true;
            this.DataMartPermissionsList.Location = new System.Drawing.Point(498, 273);
            this.DataMartPermissionsList.Name = "DataMartPermissionsList";
            this.DataMartPermissionsList.Size = new System.Drawing.Size(418, 151);
            this.DataMartPermissionsList.TabIndex = 19;
            this.DataMartPermissionsList.ThreeDCheckBoxes = true;
            // 
            // RegistryListDropDown
            // 
            this.RegistryListDropDown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.RegistryListDropDown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.RegistryListDropDown.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RegistryListDropDown.FormattingEnabled = true;
            this.RegistryListDropDown.Location = new System.Drawing.Point(230, 240);
            this.RegistryListDropDown.Name = "RegistryListDropDown";
            this.RegistryListDropDown.Size = new System.Drawing.Size(215, 28);
            this.RegistryListDropDown.TabIndex = 18;
            this.RegistryListDropDown.SelectedIndexChanged += new System.EventHandler(this.RegistryListDropDown_SelectedIndexChanged);
            // 
            // RegistryListLabel
            // 
            this.RegistryListLabel.AutoSize = true;
            this.RegistryListLabel.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RegistryListLabel.Location = new System.Drawing.Point(23, 247);
            this.RegistryListLabel.Name = "RegistryListLabel";
            this.RegistryListLabel.Size = new System.Drawing.Size(77, 25);
            this.RegistryListLabel.TabIndex = 17;
            this.RegistryListLabel.Text = "Registry";
            // 
            // RegistryPermissionsList
            // 
            this.RegistryPermissionsList.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RegistryPermissionsList.FormattingEnabled = true;
            this.RegistryPermissionsList.Location = new System.Drawing.Point(27, 273);
            this.RegistryPermissionsList.Name = "RegistryPermissionsList";
            this.RegistryPermissionsList.Size = new System.Drawing.Size(418, 151);
            this.RegistryPermissionsList.TabIndex = 16;
            this.RegistryPermissionsList.ThreeDCheckBoxes = true;
            // 
            // OrgListDropDown
            // 
            this.OrgListDropDown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.OrgListDropDown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.OrgListDropDown.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrgListDropDown.FormattingEnabled = true;
            this.OrgListDropDown.Items.AddRange(new object[] {
            "Organization A",
            "Organization B",
            "Organization C"});
            this.OrgListDropDown.Location = new System.Drawing.Point(702, 16);
            this.OrgListDropDown.Name = "OrgListDropDown";
            this.OrgListDropDown.Size = new System.Drawing.Size(214, 28);
            this.OrgListDropDown.TabIndex = 15;
            this.OrgListDropDown.SelectedIndexChanged += new System.EventHandler(this.OrgListDropDown_SelectedIndexChanged);
            // 
            // OrgListTitle
            // 
            this.OrgListTitle.AutoSize = true;
            this.OrgListTitle.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrgListTitle.Location = new System.Drawing.Point(494, 23);
            this.OrgListTitle.Name = "OrgListTitle";
            this.OrgListTitle.Size = new System.Drawing.Size(113, 25);
            this.OrgListTitle.TabIndex = 14;
            this.OrgListTitle.Text = "Organization";
            // 
            // OrgPermissionList
            // 
            this.OrgPermissionList.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrgPermissionList.FormattingEnabled = true;
            this.OrgPermissionList.Location = new System.Drawing.Point(498, 49);
            this.OrgPermissionList.Name = "OrgPermissionList";
            this.OrgPermissionList.Size = new System.Drawing.Size(418, 151);
            this.OrgPermissionList.TabIndex = 13;
            this.OrgPermissionList.ThreeDCheckBoxes = true;
            // 
            // GlobalPermissionsList
            // 
            this.GlobalPermissionsList.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GlobalPermissionsList.FormattingEnabled = true;
            this.GlobalPermissionsList.Location = new System.Drawing.Point(26, 49);
            this.GlobalPermissionsList.Name = "GlobalPermissionsList";
            this.GlobalPermissionsList.Size = new System.Drawing.Size(418, 151);
            this.GlobalPermissionsList.TabIndex = 12;
            this.GlobalPermissionsList.ThreeDCheckBoxes = true;
            // 
            // GlobalListTitle
            // 
            this.GlobalListTitle.AutoSize = true;
            this.GlobalListTitle.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GlobalListTitle.Location = new System.Drawing.Point(23, 23);
            this.GlobalListTitle.Name = "GlobalListTitle";
            this.GlobalListTitle.Size = new System.Drawing.Size(164, 25);
            this.GlobalListTitle.TabIndex = 11;
            this.GlobalListTitle.Text = "Global Permissions";
            // 
            // MainProgram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(946, 718);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainProgram";
            this.Text = "                                                                                 " +
    "                                                  ";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton FileMenu;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TextBox UserInputBox;
        private System.Windows.Forms.Label UserLabel;
        private System.Windows.Forms.Button UserFindBtn;
        private System.Windows.Forms.Label OrganizationLabel;
        private System.Windows.Forms.TextBox OrgInputBox;
        private System.Windows.Forms.Button OrgFindBtn;
        private System.Windows.Forms.CheckedListBox ProjectList;
        private System.Windows.Forms.Label ProjectListTitle;
        private System.Windows.Forms.CheckedListBox SecurityGroupList;
        private System.Windows.Forms.Label SecGroupListTitle;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label GlobalListTitle;
        private System.Windows.Forms.ComboBox OrgListDropDown;
        private System.Windows.Forms.Label OrgListTitle;
        private System.Windows.Forms.CheckedListBox OrgPermissionList;
        private System.Windows.Forms.Label FoundUsersLabel;
        private System.Windows.Forms.CheckedListBox UserList;
        private System.Windows.Forms.Button SCUBtn;
        private System.Windows.Forms.ToolStripDropDownButton Settings;
        private System.Windows.Forms.ToolStripMenuItem ChangeConnection;
        private System.Windows.Forms.ComboBox RegistryListDropDown;
        private System.Windows.Forms.Label RegistryListLabel;
        private System.Windows.Forms.CheckedListBox RegistryPermissionsList;
        private System.Windows.Forms.CheckedListBox GlobalPermissionsList;
        private System.Windows.Forms.ComboBox DataMartDropDown;
        private System.Windows.Forms.Label DataMartLabel;
        private System.Windows.Forms.CheckedListBox DataMartPermissionsList;
        private System.Windows.Forms.ComboBox UserDropDown;
        private System.Windows.Forms.Label UserPermissionsLabel;
        private System.Windows.Forms.CheckedListBox UserPermissionsList;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}