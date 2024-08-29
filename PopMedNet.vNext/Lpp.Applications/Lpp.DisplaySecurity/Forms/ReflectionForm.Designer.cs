namespace Lpp.DisplaySecurity.Forms
{
    partial class ReflectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReflectionForm));
            this.SCUBtn = new System.Windows.Forms.Button();
            this.FoundUsersLabel = new System.Windows.Forms.Label();
            this.UserList = new System.Windows.Forms.CheckedListBox();
            this.SecGroupListTitle = new System.Windows.Forms.Label();
            this.UserLabel = new System.Windows.Forms.Label();
            this.SecurityGroupList = new System.Windows.Forms.CheckedListBox();
            this.UserInputBox = new System.Windows.Forms.TextBox();
            this.OrganizationLabel = new System.Windows.Forms.Label();
            this.ProjectListTitle = new System.Windows.Forms.Label();
            this.OrgInputBox = new System.Windows.Forms.TextBox();
            this.ProjectList = new System.Windows.Forms.CheckedListBox();
            this.UserFindBtn = new System.Windows.Forms.Button();
            this.OrgFindBtn = new System.Windows.Forms.Button();
            this.GlobalPermissionsList = new System.Windows.Forms.CheckedListBox();
            this.GlobalListTitle = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.File = new System.Windows.Forms.ToolStripDropDownButton();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsMenu = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SCUBtn
            // 
            this.SCUBtn.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SCUBtn.Location = new System.Drawing.Point(738, 10);
            this.SCUBtn.Name = "SCUBtn";
            this.SCUBtn.Size = new System.Drawing.Size(160, 22);
            this.SCUBtn.TabIndex = 26;
            this.SCUBtn.Text = "Select Checked Users";
            this.SCUBtn.UseVisualStyleBackColor = true;
            this.SCUBtn.Click += new System.EventHandler(this.SCUBtn_Click_1);
            // 
            // FoundUsersLabel
            // 
            this.FoundUsersLabel.AutoSize = true;
            this.FoundUsersLabel.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FoundUsersLabel.Location = new System.Drawing.Point(487, 5);
            this.FoundUsersLabel.Name = "FoundUsersLabel";
            this.FoundUsersLabel.Size = new System.Drawing.Size(114, 25);
            this.FoundUsersLabel.TabIndex = 25;
            this.FoundUsersLabel.Text = "Found Users";
            // 
            // UserList
            // 
            this.UserList.CheckOnClick = true;
            this.UserList.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserList.FormattingEnabled = true;
            this.UserList.Location = new System.Drawing.Point(492, 35);
            this.UserList.Name = "UserList";
            this.UserList.Size = new System.Drawing.Size(406, 109);
            this.UserList.TabIndex = 24;
            this.UserList.SelectedIndexChanged += new System.EventHandler(this.UserList_SelectedIndexChanged);
            // 
            // SecGroupListTitle
            // 
            this.SecGroupListTitle.AutoSize = true;
            this.SecGroupListTitle.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SecGroupListTitle.Location = new System.Drawing.Point(487, 147);
            this.SecGroupListTitle.Name = "SecGroupListTitle";
            this.SecGroupListTitle.Size = new System.Drawing.Size(139, 25);
            this.SecGroupListTitle.TabIndex = 23;
            this.SecGroupListTitle.Text = "Security Groups";
            // 
            // UserLabel
            // 
            this.UserLabel.AutoSize = true;
            this.UserLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserLabel.Location = new System.Drawing.Point(19, 10);
            this.UserLabel.Name = "UserLabel";
            this.UserLabel.Size = new System.Drawing.Size(76, 20);
            this.UserLabel.TabIndex = 15;
            this.UserLabel.Text = "User Name";
            // 
            // SecurityGroupList
            // 
            this.SecurityGroupList.CheckOnClick = true;
            this.SecurityGroupList.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SecurityGroupList.FormattingEnabled = true;
            this.SecurityGroupList.Location = new System.Drawing.Point(491, 175);
            this.SecurityGroupList.Name = "SecurityGroupList";
            this.SecurityGroupList.Size = new System.Drawing.Size(406, 151);
            this.SecurityGroupList.TabIndex = 22;
            this.SecurityGroupList.ThreeDCheckBoxes = true;
            // 
            // UserInputBox
            // 
            this.UserInputBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserInputBox.Location = new System.Drawing.Point(151, 10);
            this.UserInputBox.Name = "UserInputBox";
            this.UserInputBox.Size = new System.Drawing.Size(227, 22);
            this.UserInputBox.TabIndex = 14;
            // 
            // OrganizationLabel
            // 
            this.OrganizationLabel.AutoSize = true;
            this.OrganizationLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrganizationLabel.Location = new System.Drawing.Point(19, 52);
            this.OrganizationLabel.Name = "OrganizationLabel";
            this.OrganizationLabel.Size = new System.Drawing.Size(122, 20);
            this.OrganizationLabel.TabIndex = 17;
            this.OrganizationLabel.Text = "Organization Name";
            // 
            // ProjectListTitle
            // 
            this.ProjectListTitle.AutoSize = true;
            this.ProjectListTitle.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProjectListTitle.Location = new System.Drawing.Point(18, 149);
            this.ProjectListTitle.Name = "ProjectListTitle";
            this.ProjectListTitle.Size = new System.Drawing.Size(76, 25);
            this.ProjectListTitle.TabIndex = 21;
            this.ProjectListTitle.Text = "Projects";
            // 
            // OrgInputBox
            // 
            this.OrgInputBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrgInputBox.Location = new System.Drawing.Point(151, 52);
            this.OrgInputBox.Name = "OrgInputBox";
            this.OrgInputBox.Size = new System.Drawing.Size(227, 22);
            this.OrgInputBox.TabIndex = 18;
            // 
            // ProjectList
            // 
            this.ProjectList.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProjectList.FormattingEnabled = true;
            this.ProjectList.Location = new System.Drawing.Point(23, 175);
            this.ProjectList.Name = "ProjectList";
            this.ProjectList.Size = new System.Drawing.Size(404, 151);
            this.ProjectList.TabIndex = 20;
            this.ProjectList.ThreeDCheckBoxes = true;
            // 
            // UserFindBtn
            // 
            this.UserFindBtn.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserFindBtn.Location = new System.Drawing.Point(384, 10);
            this.UserFindBtn.Name = "UserFindBtn";
            this.UserFindBtn.Size = new System.Drawing.Size(87, 22);
            this.UserFindBtn.TabIndex = 16;
            this.UserFindBtn.Text = "Find";
            this.UserFindBtn.UseVisualStyleBackColor = true;
            this.UserFindBtn.Click += new System.EventHandler(this.UserFindBtn_Click);
            // 
            // OrgFindBtn
            // 
            this.OrgFindBtn.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrgFindBtn.Location = new System.Drawing.Point(384, 52);
            this.OrgFindBtn.Name = "OrgFindBtn";
            this.OrgFindBtn.Size = new System.Drawing.Size(87, 22);
            this.OrgFindBtn.TabIndex = 19;
            this.OrgFindBtn.Text = "Find";
            this.OrgFindBtn.UseVisualStyleBackColor = true;
            this.OrgFindBtn.Click += new System.EventHandler(this.OrgFindBtn_Click_1);
            // 
            // GlobalPermissionsList
            // 
            this.GlobalPermissionsList.CheckOnClick = true;
            this.GlobalPermissionsList.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GlobalPermissionsList.FormattingEnabled = true;
            this.GlobalPermissionsList.Location = new System.Drawing.Point(21, 30);
            this.GlobalPermissionsList.Name = "GlobalPermissionsList";
            this.GlobalPermissionsList.Size = new System.Drawing.Size(404, 151);
            this.GlobalPermissionsList.TabIndex = 28;
            this.GlobalPermissionsList.ThreeDCheckBoxes = true;
            this.GlobalPermissionsList.SelectedIndexChanged += new System.EventHandler(this.GlobalPermissionsList_SelectedIndexChanged);
            // 
            // GlobalListTitle
            // 
            this.GlobalListTitle.AutoSize = true;
            this.GlobalListTitle.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GlobalListTitle.Location = new System.Drawing.Point(18, 2);
            this.GlobalListTitle.Name = "GlobalListTitle";
            this.GlobalListTitle.Size = new System.Drawing.Size(164, 25);
            this.GlobalListTitle.TabIndex = 27;
            this.GlobalListTitle.Text = "Global Permissions";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Location = new System.Drawing.Point(3, 28);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.SCUBtn);
            this.splitContainer1.Panel1.Controls.Add(this.UserInputBox);
            this.splitContainer1.Panel1.Controls.Add(this.FoundUsersLabel);
            this.splitContainer1.Panel1.Controls.Add(this.OrgFindBtn);
            this.splitContainer1.Panel1.Controls.Add(this.UserList);
            this.splitContainer1.Panel1.Controls.Add(this.UserFindBtn);
            this.splitContainer1.Panel1.Controls.Add(this.SecGroupListTitle);
            this.splitContainer1.Panel1.Controls.Add(this.OrgInputBox);
            this.splitContainer1.Panel1.Controls.Add(this.SecurityGroupList);
            this.splitContainer1.Panel1.Controls.Add(this.UserLabel);
            this.splitContainer1.Panel1.Controls.Add(this.ProjectListTitle);
            this.splitContainer1.Panel1.Controls.Add(this.OrganizationLabel);
            this.splitContainer1.Panel1.Controls.Add(this.ProjectList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.GlobalListTitle);
            this.splitContainer1.Panel2.Controls.Add(this.GlobalPermissionsList);
            this.splitContainer1.Size = new System.Drawing.Size(937, 847);
            this.splitContainer1.SplitterDistance = 347;
            this.splitContainer1.TabIndex = 29;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File,
            this.SettingsMenu});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(945, 25);
            this.toolStrip1.TabIndex = 30;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // File
            // 
            this.File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.File.Image = ((System.Drawing.Image)(resources.GetObject("File.Image")));
            this.File.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.File.Name = "File";
            this.File.Size = new System.Drawing.Size(38, 22);
            this.File.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click_1);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click_1);
            // 
            // SettingsMenu
            // 
            this.SettingsMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SettingsMenu.Image = ((System.Drawing.Image)(resources.GetObject("SettingsMenu.Image")));
            this.SettingsMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SettingsMenu.Name = "SettingsMenu";
            this.SettingsMenu.Size = new System.Drawing.Size(53, 22);
            this.SettingsMenu.Text = "Settings";
            this.SettingsMenu.Click += new System.EventHandler(this.SettingsMenu_Click);
            // 
            // ReflectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScrollMargin = new System.Drawing.Size(2, 5);
            this.ClientSize = new System.Drawing.Size(945, 880);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ReflectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Security and Permissions";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SCUBtn;
        private System.Windows.Forms.Label FoundUsersLabel;
        private System.Windows.Forms.CheckedListBox UserList;
        private System.Windows.Forms.Label SecGroupListTitle;
        private System.Windows.Forms.Label UserLabel;
        private System.Windows.Forms.CheckedListBox SecurityGroupList;
        private System.Windows.Forms.TextBox UserInputBox;
        private System.Windows.Forms.Label OrganizationLabel;
        private System.Windows.Forms.Label ProjectListTitle;
        private System.Windows.Forms.TextBox OrgInputBox;
        private System.Windows.Forms.CheckedListBox ProjectList;
        private System.Windows.Forms.Button UserFindBtn;
        private System.Windows.Forms.Button OrgFindBtn;
        private System.Windows.Forms.CheckedListBox GlobalPermissionsList;
        private System.Windows.Forms.Label GlobalListTitle;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton File;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton SettingsMenu;

    }
}