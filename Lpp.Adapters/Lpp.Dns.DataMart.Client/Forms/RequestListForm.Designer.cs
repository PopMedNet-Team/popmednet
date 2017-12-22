using System;
namespace Lpp.Dns.DataMart.Client
{
    partial class RequestListForm
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
            if (disposing)
            {
                Lpp.Dns.DataMart.Client.Utils.SystemTray.DisposeSystemTrayIcon();
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.LinkLabel openSettings;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RequestListForm));
            this.initializeProgress = new System.Windows.Forms.ProgressBar();
            this.listContainer = new System.Windows.Forms.Panel();
            this.splash = new System.Windows.Forms.Panel();
            this.noNetworks = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.cbRunAtWindowsStartup = new System.Windows.Forms.CheckBox();
            this.btnNetworkSettings = new System.Windows.Forms.Button();
            this.cbAutoRefresh = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnViewDetail = new System.Windows.Forms.Button();
            this.tabs = new System.Windows.Forms.TabControl();
            this.startupWorker = new System.ComponentModel.BackgroundWorker();
            this.toolStripStatusNetworkConnectivityStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripLoginStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblAbout = new System.Windows.Forms.ToolStripStatusLabel();
            label1 = new System.Windows.Forms.Label();
            openSettings = new System.Windows.Forms.LinkLabel();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            this.listContainer.SuspendLayout();
            this.splash.SuspendLayout();
            this.noNetworks.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            label1.Location = new System.Drawing.Point(119, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(570, 29);
            label1.TabIndex = 0;
            label1.Text = "Please stand by while DataMart Client is initializing...";
            // 
            // openSettings
            // 
            openSettings.Anchor = System.Windows.Forms.AnchorStyles.None;
            openSettings.AutoSize = true;
            openSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            openSettings.Location = new System.Drawing.Point(379, 84);
            openSettings.Name = "openSettings";
            openSettings.Size = new System.Drawing.Size(179, 29);
            openSettings.TabIndex = 4;
            openSettings.TabStop = true;
            openSettings.Text = "Open Settings";
            openSettings.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.openSettings_LinkClicked);
            // 
            // label3
            // 
            label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            label3.Location = new System.Drawing.Point(159, 46);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(633, 25);
            label3.TabIndex = 2;
            label3.Text = "To get started, please open settings and set up your network connection";
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            label2.ForeColor = System.Drawing.Color.Maroon;
            label2.Location = new System.Drawing.Point(345, 14);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(240, 29);
            label2.TabIndex = 3;
            label2.Text = "No Networks defined";
            // 
            // initializeProgress
            // 
            this.initializeProgress.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.initializeProgress.Location = new System.Drawing.Point(124, 42);
            this.initializeProgress.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.initializeProgress.MarqueeAnimationSpeed = 20;
            this.initializeProgress.Name = "initializeProgress";
            this.initializeProgress.Size = new System.Drawing.Size(559, 23);
            this.initializeProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.initializeProgress.TabIndex = 1;
            // 
            // listContainer
            // 
            this.listContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listContainer.Controls.Add(this.splash);
            this.listContainer.Controls.Add(this.noNetworks);
            this.listContainer.Controls.Add(this.btnExit);
            this.listContainer.Controls.Add(this.cbRunAtWindowsStartup);
            this.listContainer.Controls.Add(this.btnNetworkSettings);
            this.listContainer.Controls.Add(this.cbAutoRefresh);
            this.listContainer.Controls.Add(this.btnClose);
            this.listContainer.Controls.Add(this.btnRefresh);
            this.listContainer.Controls.Add(this.btnViewDetail);
            this.listContainer.Controls.Add(this.tabs);
            this.listContainer.Location = new System.Drawing.Point(4, 4);
            this.listContainer.Margin = new System.Windows.Forms.Padding(4);
            this.listContainer.Name = "listContainer";
            this.listContainer.Padding = new System.Windows.Forms.Padding(4);
            this.listContainer.Size = new System.Drawing.Size(1324, 587);
            this.listContainer.TabIndex = 21;
            // 
            // splash
            // 
            this.splash.Controls.Add(this.initializeProgress);
            this.splash.Controls.Add(label1);
            this.splash.Location = new System.Drawing.Point(35, 37);
            this.splash.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splash.Name = "splash";
            this.splash.Size = new System.Drawing.Size(809, 194);
            this.splash.TabIndex = 22;
            // 
            // noNetworks
            // 
            this.noNetworks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noNetworks.Controls.Add(openSettings);
            this.noNetworks.Controls.Add(label3);
            this.noNetworks.Controls.Add(label2);
            this.noNetworks.Location = new System.Drawing.Point(60, 300);
            this.noNetworks.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.noNetworks.Name = "noNetworks";
            this.noNetworks.Size = new System.Drawing.Size(949, 194);
            this.noNetworks.TabIndex = 23;
            this.noNetworks.Visible = false;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnExit.AutoSize = true;
            this.btnExit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(905, 542);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(47, 30);
            this.btnExit.TabIndex = 24;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.ExitApplication_EventHandler);
            // 
            // cbRunAtWindowsStartup
            // 
            this.cbRunAtWindowsStartup.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cbRunAtWindowsStartup.AutoSize = true;
            this.cbRunAtWindowsStartup.Checked = true;
            this.cbRunAtWindowsStartup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRunAtWindowsStartup.Location = new System.Drawing.Point(205, 547);
            this.cbRunAtWindowsStartup.Margin = new System.Windows.Forms.Padding(4);
            this.cbRunAtWindowsStartup.Name = "cbRunAtWindowsStartup";
            this.cbRunAtWindowsStartup.Size = new System.Drawing.Size(148, 21);
            this.cbRunAtWindowsStartup.TabIndex = 28;
            this.cbRunAtWindowsStartup.Text = "Start with Windows";
            this.cbRunAtWindowsStartup.UseVisualStyleBackColor = true;
            this.cbRunAtWindowsStartup.CheckedChanged += new System.EventHandler(this.cbRunAtWindowsStartup_CheckedChanged);
            // 
            // btnNetworkSettings
            // 
            this.btnNetworkSettings.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnNetworkSettings.AutoSize = true;
            this.btnNetworkSettings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnNetworkSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNetworkSettings.Location = new System.Drawing.Point(721, 542);
            this.btnNetworkSettings.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNetworkSettings.Name = "btnNetworkSettings";
            this.btnNetworkSettings.Size = new System.Drawing.Size(80, 30);
            this.btnNetworkSettings.TabIndex = 27;
            this.btnNetworkSettings.Text = "Settings";
            this.btnNetworkSettings.UseVisualStyleBackColor = true;
            this.btnNetworkSettings.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // cbAutoRefresh
            // 
            this.cbAutoRefresh.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cbAutoRefresh.AutoSize = true;
            this.cbAutoRefresh.Checked = true;
            this.cbAutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoRefresh.Location = new System.Drawing.Point(363, 547);
            this.cbAutoRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.cbAutoRefresh.Name = "cbAutoRefresh";
            this.cbAutoRefresh.Size = new System.Drawing.Size(146, 21);
            this.cbAutoRefresh.TabIndex = 28;
            this.cbAutoRefresh.Text = "Automatic Refresh";
            this.cbAutoRefresh.UseVisualStyleBackColor = true;
            this.cbAutoRefresh.CheckedChanged += new System.EventHandler(this.cbAutoRefresh_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.AutoSize = true;
            this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(813, 542);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(62, 30);
            this.btnClose.TabIndex = 26;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRefresh.AutoSize = true;
            this.btnRefresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(537, 542);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(78, 30);
            this.btnRefresh.TabIndex = 22;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.Refresh_EventHandler);
            // 
            // btnViewDetail
            // 
            this.btnViewDetail.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnViewDetail.AutoSize = true;
            this.btnViewDetail.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnViewDetail.Enabled = false;
            this.btnViewDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewDetail.Location = new System.Drawing.Point(629, 542);
            this.btnViewDetail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnViewDetail.Name = "btnViewDetail";
            this.btnViewDetail.Size = new System.Drawing.Size(72, 30);
            this.btnViewDetail.TabIndex = 23;
            this.btnViewDetail.Text = "Details";
            this.btnViewDetail.UseVisualStyleBackColor = true;
            this.btnViewDetail.Click += new System.EventHandler(this.btnViewDetail_Click);
            // 
            // tabs
            // 
            this.tabs.Location = new System.Drawing.Point(84, 64);
            this.tabs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.ShowToolTips = true;
            this.tabs.Size = new System.Drawing.Size(1077, 460);
            this.tabs.TabIndex = 29;
            // 
            // startupWorker
            // 
            this.startupWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.startupWorker_DoWork);
            this.startupWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.startupWorker_RunWorkerCompleted);
            // 
            // toolStripStatusNetworkConnectivityStatus
            // 
            this.toolStripStatusNetworkConnectivityStatus.AutoSize = false;
            this.toolStripStatusNetworkConnectivityStatus.AutoToolTip = true;
            this.toolStripStatusNetworkConnectivityStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusNetworkConnectivityStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusNetworkConnectivityStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripStatusNetworkConnectivityStatus.Name = "toolStripStatusNetworkConnectivityStatus";
            this.toolStripStatusNetworkConnectivityStatus.Size = new System.Drawing.Size(1231, 24);
            this.toolStripStatusNetworkConnectivityStatus.Spring = true;
            this.toolStripStatusNetworkConnectivityStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripLoginStatus
            // 
            this.toolStripLoginStatus.AutoToolTip = true;
            this.toolStripLoginStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripLoginStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripLoginStatus.Name = "toolStripLoginStatus";
            this.toolStripLoginStatus.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripLoginStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripLoginStatus.Size = new System.Drawing.Size(2, 24);
            this.toolStripLoginStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusNetworkConnectivityStatus,
            this.toolStripLoginStatus,
            this.lblAbout});
            this.statusStrip1.Location = new System.Drawing.Point(0, 600);
            this.statusStrip1.MinimumSize = new System.Drawing.Size(1069, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1331, 29);
            this.statusStrip1.TabIndex = 20;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblAbout
            // 
            this.lblAbout.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblAbout.Image = ((System.Drawing.Image)(resources.GetObject("lblAbout.Image")));
            this.lblAbout.IsLink = true;
            this.lblAbout.Name = "lblAbout";
            this.lblAbout.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAbout.Size = new System.Drawing.Size(78, 24);
            this.lblAbout.Text = "About";
            this.lblAbout.Click += new System.EventHandler(this.lblAbout_Click);
            // 
            // RequestListForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1331, 629);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.listContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1346, 665);
            this.Name = "RequestListForm";
            this.Text = "DataMart Client";
            this.Load += new System.EventHandler(this.RequestListForm_Load);
            this.listContainer.ResumeLayout(false);
            this.listContainer.PerformLayout();
            this.splash.ResumeLayout(false);
            this.splash.PerformLayout();
            this.noNetworks.ResumeLayout(false);
            this.noNetworks.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel listContainer;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnViewDetail;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnNetworkSettings;
        private System.Windows.Forms.CheckBox cbRunAtWindowsStartup;
        private System.ComponentModel.BackgroundWorker startupWorker;
        private System.Windows.Forms.CheckBox cbAutoRefresh;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusNetworkConnectivityStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripLoginStatus;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.Panel splash;
        private System.Windows.Forms.Panel noNetworks;
        private System.Windows.Forms.ProgressBar initializeProgress;
        private System.Windows.Forms.ToolStripStatusLabel lblAbout;
    }
}

