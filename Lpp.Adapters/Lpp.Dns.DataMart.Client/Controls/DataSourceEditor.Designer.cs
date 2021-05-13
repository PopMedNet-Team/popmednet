namespace Lpp.Dns.DataMart.Client.Controls
{
    partial class DataSourceEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpDataSource = new System.Windows.Forms.GroupBox();
            this.pnlDirect = new System.Windows.Forms.Panel();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.txtCommandTimeout = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtConnectionTimeout = new System.Windows.Forms.NumericUpDown();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlODBC = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbDataSourceName = new System.Windows.Forms.ComboBox();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.cmbDataProvider = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkEncrypt = new System.Windows.Forms.CheckBox();
            this.grpDataSource.SuspendLayout();
            this.pnlDirect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCommandTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtConnectionTimeout)).BeginInit();
            this.pnlODBC.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpDataSource
            // 
            this.grpDataSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDataSource.Controls.Add(this.pnlDirect);
            this.grpDataSource.Controls.Add(this.pnlODBC);
            this.grpDataSource.Controls.Add(this.btnTestConnection);
            this.grpDataSource.Controls.Add(this.cmbDataProvider);
            this.grpDataSource.Controls.Add(this.label1);
            this.grpDataSource.Location = new System.Drawing.Point(3, 3);
            this.grpDataSource.Name = "grpDataSource";
            this.grpDataSource.Size = new System.Drawing.Size(384, 266);
            this.grpDataSource.TabIndex = 0;
            this.grpDataSource.TabStop = false;
            this.grpDataSource.Text = "Data Source";
            // 
            // pnlDirect
            // 
            this.pnlDirect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDirect.Controls.Add(this.chkEncrypt);
            this.pnlDirect.Controls.Add(this.txtPort);
            this.pnlDirect.Controls.Add(this.label8);
            this.pnlDirect.Controls.Add(this.txtServer);
            this.pnlDirect.Controls.Add(this.lblServer);
            this.pnlDirect.Controls.Add(this.txtUserID);
            this.pnlDirect.Controls.Add(this.txtCommandTimeout);
            this.pnlDirect.Controls.Add(this.label3);
            this.pnlDirect.Controls.Add(this.label7);
            this.pnlDirect.Controls.Add(this.txtConnectionTimeout);
            this.pnlDirect.Controls.Add(this.txtPassword);
            this.pnlDirect.Controls.Add(this.label4);
            this.pnlDirect.Controls.Add(this.label6);
            this.pnlDirect.Controls.Add(this.txtDatabase);
            this.pnlDirect.Controls.Add(this.label5);
            this.pnlDirect.Location = new System.Drawing.Point(11, 41);
            this.pnlDirect.Name = "pnlDirect";
            this.pnlDirect.Size = new System.Drawing.Size(367, 172);
            this.pnlDirect.TabIndex = 18;
            // 
            // txtPort
            // 
            this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPort.Location = new System.Drawing.Point(291, 3);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(73, 20);
            this.txtPort.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(256, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Port:";
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Location = new System.Drawing.Point(75, 3);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(175, 20);
            this.txtServer.TabIndex = 0;
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(28, 6);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(41, 13);
            this.lblServer.TabIndex = 2;
            this.lblServer.Text = "Server:";
            // 
            // txtUserID
            // 
            this.txtUserID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserID.Location = new System.Drawing.Point(75, 29);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(289, 20);
            this.txtUserID.TabIndex = 2;
            // 
            // txtCommandTimeout
            // 
            this.txtCommandTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommandTimeout.Location = new System.Drawing.Point(291, 107);
            this.txtCommandTimeout.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.txtCommandTimeout.Name = "txtCommandTimeout";
            this.txtCommandTimeout.Size = new System.Drawing.Size(71, 20);
            this.txtCommandTimeout.TabIndex = 6;
            this.txtCommandTimeout.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "User ID:";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(187, 109);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Command Timeout:";
            // 
            // txtConnectionTimeout
            // 
            this.txtConnectionTimeout.Location = new System.Drawing.Point(114, 107);
            this.txtConnectionTimeout.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.txtConnectionTimeout.Name = "txtConnectionTimeout";
            this.txtConnectionTimeout.Size = new System.Drawing.Size(71, 20);
            this.txtConnectionTimeout.TabIndex = 5;
            this.txtConnectionTimeout.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(75, 55);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(289, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Password:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Connection Timeout:";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDatabase.Location = new System.Drawing.Point(75, 81);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(289, 20);
            this.txtDatabase.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Database:";
            // 
            // pnlODBC
            // 
            this.pnlODBC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlODBC.Controls.Add(this.label2);
            this.pnlODBC.Controls.Add(this.cmbDataSourceName);
            this.pnlODBC.Location = new System.Drawing.Point(11, 41);
            this.pnlODBC.Name = "pnlODBC";
            this.pnlODBC.Size = new System.Drawing.Size(367, 29);
            this.pnlODBC.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "ODBC DSN:";
            // 
            // cmbDataSourceName
            // 
            this.cmbDataSourceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDataSourceName.FormattingEnabled = true;
            this.cmbDataSourceName.Location = new System.Drawing.Point(75, 3);
            this.cmbDataSourceName.Name = "cmbDataSourceName";
            this.cmbDataSourceName.Size = new System.Drawing.Size(289, 21);
            this.cmbDataSourceName.TabIndex = 5;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestConnection.Location = new System.Drawing.Point(206, 220);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(172, 40);
            this.btnTestConnection.TabIndex = 1;
            this.btnTestConnection.Text = "Test";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // cmbDataProvider
            // 
            this.cmbDataProvider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDataProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataProvider.FormattingEnabled = true;
            this.cmbDataProvider.Location = new System.Drawing.Point(87, 19);
            this.cmbDataProvider.Name = "cmbDataProvider";
            this.cmbDataProvider.Size = new System.Drawing.Size(291, 21);
            this.cmbDataProvider.TabIndex = 0;
            this.cmbDataProvider.SelectedIndexChanged += new System.EventHandler(this.cmbDataProvider_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data Provider:";
            // 
            // chkEncrypt
            // 
            this.chkEncrypt.AutoSize = true;
            this.chkEncrypt.Location = new System.Drawing.Point(6, 133);
            this.chkEncrypt.Name = "chkEncrypt";
            this.chkEncrypt.Size = new System.Drawing.Size(119, 17);
            this.chkEncrypt.TabIndex = 18;
            this.chkEncrypt.Text = "Encrypt Connection";
            this.chkEncrypt.UseVisualStyleBackColor = true;
            // 
            // DataSourceEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpDataSource);
            this.MinimumSize = new System.Drawing.Size(390, 230);
            this.Name = "DataSourceEditor";
            this.Size = new System.Drawing.Size(390, 271);
            this.Load += new System.EventHandler(this.DataSourceEditor_Load);
            this.grpDataSource.ResumeLayout(false);
            this.grpDataSource.PerformLayout();
            this.pnlDirect.ResumeLayout(false);
            this.pnlDirect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCommandTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtConnectionTimeout)).EndInit();
            this.pnlODBC.ResumeLayout(false);
            this.pnlODBC.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpDataSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.ComboBox cmbDataProvider;
        private System.Windows.Forms.ComboBox cmbDataSourceName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.NumericUpDown txtConnectionTimeout;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.NumericUpDown txtCommandTimeout;
        private System.Windows.Forms.Panel pnlDirect;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel pnlODBC;
        private System.Windows.Forms.CheckBox chkEncrypt;
    }
}
