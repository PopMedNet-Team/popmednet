namespace Lpp.Dns.DataMart.Client
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.button1 = new System.Windows.Forms.Button();
            this.lblStartingUp = new System.Windows.Forms.Label();
            this.lblArch = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDebug = new System.Windows.Forms.Button();
            this.btnManagePackages = new System.Windows.Forms.Button();
            this.btnUpdateRefreshRate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(457, 159);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 12;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblStartingUp
            // 
            this.lblStartingUp.AutoSize = true;
            this.lblStartingUp.Location = new System.Drawing.Point(195, 135);
            this.lblStartingUp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStartingUp.Name = "lblStartingUp";
            this.lblStartingUp.Size = new System.Drawing.Size(168, 17);
            this.lblStartingUp.TabIndex = 21;
            this.lblStartingUp.Text = "Starting up... Please wait.";
            this.lblStartingUp.Visible = false;
            // 
            // lblArch
            // 
            this.lblArch.AutoSize = true;
            this.lblArch.Location = new System.Drawing.Point(245, 80);
            this.lblArch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblArch.Name = "lblArch";
            this.lblArch.Size = new System.Drawing.Size(51, 17);
            this.lblArch.TabIndex = 19;
            this.lblArch.Text = "lblArch";
            this.lblArch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(245, 110);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(52, 17);
            this.lblVersion.TabIndex = 17;
            this.lblVersion.Text = "0.0.0.0";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(23, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(502, 51);
            this.label4.TabIndex = 16;
            this.label4.Text = "DataMart Client powered by PopMedNet(tm)\r\nDistributed Research Network Technologi" +
    "es for Population Medicine\r\n\r\n";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDebug
            // 
            this.btnDebug.Location = new System.Drawing.Point(16, 159);
            this.btnDebug.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(100, 28);
            this.btnDebug.TabIndex = 22;
            this.btnDebug.Text = "Debug";
            this.btnDebug.UseVisualStyleBackColor = true;
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
            // 
            // btnManagePackages
            // 
            this.btnManagePackages.Location = new System.Drawing.Point(124, 159);
            this.btnManagePackages.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnManagePackages.Name = "btnManagePackages";
            this.btnManagePackages.Size = new System.Drawing.Size(100, 28);
            this.btnManagePackages.TabIndex = 24;
            this.btnManagePackages.Text = "Packages";
            this.btnManagePackages.UseVisualStyleBackColor = true;
            this.btnManagePackages.Click += new System.EventHandler(this.btnManagePackages_Click);
            // 
            // btnUpdateRefreshRate
            // 
            this.btnUpdateRefreshRate.Location = new System.Drawing.Point(233, 158);
            this.btnUpdateRefreshRate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUpdateRefreshRate.Name = "btnUpdateRefreshRate";
            this.btnUpdateRefreshRate.Size = new System.Drawing.Size(167, 28);
            this.btnUpdateRefreshRate.TabIndex = 25;
            this.btnUpdateRefreshRate.Text = "Update Refresh Rate";
            this.btnUpdateRefreshRate.UseVisualStyleBackColor = true;
            this.btnUpdateRefreshRate.Click += new System.EventHandler(this.btnUpdateRefreshRate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 53);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 17);
            this.label2.TabIndex = 26;
            this.label2.Text = "www.popmednet.org";
            // 
            // AboutForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(572, 201);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnUpdateRefreshRate);
            this.Controls.Add(this.btnManagePackages);
            this.Controls.Add(this.btnDebug);
            this.Controls.Add(this.lblStartingUp);
            this.Controls.Add(this.lblArch);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DataMart Client";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblStartingUp;
        private System.Windows.Forms.Label lblArch;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDebug;
        private System.Windows.Forms.Button btnManagePackages;
        private System.Windows.Forms.Button btnUpdateRefreshRate;
        private System.Windows.Forms.Label label2;
    }
}