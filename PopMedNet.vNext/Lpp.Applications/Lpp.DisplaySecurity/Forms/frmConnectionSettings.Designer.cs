namespace Lpp.DisplaySecurity.Forms
{
    partial class frmConnectionSettings
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
            this.NewConnectionPanel = new System.Windows.Forms.Panel();
            this.TrustedConnectionChkBox = new System.Windows.Forms.CheckBox();
            this.TCLabel = new System.Windows.Forms.Label();
            this.CommandInput = new System.Windows.Forms.NumericUpDown();
            this.ConnectInput = new System.Windows.Forms.NumericUpDown();
            this.CommandLabel = new System.Windows.Forms.Label();
            this.ConnectionLabel = new System.Windows.Forms.Label();
            this.PortLabel = new System.Windows.Forms.Label();
            this.UserIDLabel = new System.Windows.Forms.Label();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.DatabaseLabel = new System.Windows.Forms.Label();
            this.ServerLabel = new System.Windows.Forms.Label();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.UserIDTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.DatabaseTextBox = new System.Windows.Forms.TextBox();
            this.ServerTextBox = new System.Windows.Forms.TextBox();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.SavedConnections = new System.Windows.Forms.ComboBox();
            this.SavedLabel = new System.Windows.Forms.Label();
            this.btnSetDefault = new System.Windows.Forms.Button();
            this.SelectedConnection = new System.Windows.Forms.TextBox();
            this.SelectedConnectionLabel = new System.Windows.Forms.Label();
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.NewConnectionPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CommandInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectInput)).BeginInit();
            this.SuspendLayout();
            // 
            // NewConnectionPanel
            // 
            this.NewConnectionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NewConnectionPanel.Controls.Add(this.TrustedConnectionChkBox);
            this.NewConnectionPanel.Controls.Add(this.TCLabel);
            this.NewConnectionPanel.Controls.Add(this.CommandInput);
            this.NewConnectionPanel.Controls.Add(this.ConnectInput);
            this.NewConnectionPanel.Controls.Add(this.CommandLabel);
            this.NewConnectionPanel.Controls.Add(this.ConnectionLabel);
            this.NewConnectionPanel.Controls.Add(this.PortLabel);
            this.NewConnectionPanel.Controls.Add(this.UserIDLabel);
            this.NewConnectionPanel.Controls.Add(this.PasswordLabel);
            this.NewConnectionPanel.Controls.Add(this.DatabaseLabel);
            this.NewConnectionPanel.Controls.Add(this.ServerLabel);
            this.NewConnectionPanel.Controls.Add(this.PortTextBox);
            this.NewConnectionPanel.Controls.Add(this.UserIDTextBox);
            this.NewConnectionPanel.Controls.Add(this.PasswordTextBox);
            this.NewConnectionPanel.Controls.Add(this.DatabaseTextBox);
            this.NewConnectionPanel.Controls.Add(this.ServerTextBox);
            this.NewConnectionPanel.Location = new System.Drawing.Point(12, 96);
            this.NewConnectionPanel.Name = "NewConnectionPanel";
            this.NewConnectionPanel.Size = new System.Drawing.Size(628, 266);
            this.NewConnectionPanel.TabIndex = 0;
            // 
            // TrustedConnectionChkBox
            // 
            this.TrustedConnectionChkBox.AutoSize = true;
            this.TrustedConnectionChkBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TrustedConnectionChkBox.Location = new System.Drawing.Point(158, 226);
            this.TrustedConnectionChkBox.Name = "TrustedConnectionChkBox";
            this.TrustedConnectionChkBox.Size = new System.Drawing.Size(15, 14);
            this.TrustedConnectionChkBox.TabIndex = 19;
            this.TrustedConnectionChkBox.UseVisualStyleBackColor = true;
            this.TrustedConnectionChkBox.CheckedChanged += new System.EventHandler(this.TrustedConnectionChkBox_CheckedChanged);
            // 
            // TCLabel
            // 
            this.TCLabel.AutoSize = true;
            this.TCLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TCLabel.Location = new System.Drawing.Point(24, 220);
            this.TCLabel.Name = "TCLabel";
            this.TCLabel.Size = new System.Drawing.Size(128, 20);
            this.TCLabel.TabIndex = 16;
            this.TCLabel.Text = "Trusted Connection:";
            // 
            // CommandInput
            // 
            this.CommandInput.Location = new System.Drawing.Point(434, 181);
            this.CommandInput.Name = "CommandInput";
            this.CommandInput.Size = new System.Drawing.Size(132, 20);
            this.CommandInput.TabIndex = 14;
            // 
            // ConnectInput
            // 
            this.ConnectInput.Location = new System.Drawing.Point(170, 181);
            this.ConnectInput.Name = "ConnectInput";
            this.ConnectInput.Size = new System.Drawing.Size(113, 20);
            this.ConnectInput.TabIndex = 13;
            // 
            // CommandLabel
            // 
            this.CommandLabel.AutoSize = true;
            this.CommandLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CommandLabel.Location = new System.Drawing.Point(303, 181);
            this.CommandLabel.Name = "CommandLabel";
            this.CommandLabel.Size = new System.Drawing.Size(125, 20);
            this.CommandLabel.TabIndex = 12;
            this.CommandLabel.Text = "Command Timeout:";
            // 
            // ConnectionLabel
            // 
            this.ConnectionLabel.AutoSize = true;
            this.ConnectionLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectionLabel.Location = new System.Drawing.Point(24, 181);
            this.ConnectionLabel.Name = "ConnectionLabel";
            this.ConnectionLabel.Size = new System.Drawing.Size(131, 20);
            this.ConnectionLabel.TabIndex = 11;
            this.ConnectionLabel.Text = "Connection Timeout:";
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PortLabel.Location = new System.Drawing.Point(420, 28);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(37, 20);
            this.PortLabel.TabIndex = 10;
            this.PortLabel.Text = "Port:";
            // 
            // UserIDLabel
            // 
            this.UserIDLabel.AutoSize = true;
            this.UserIDLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserIDLabel.Location = new System.Drawing.Point(39, 61);
            this.UserIDLabel.Name = "UserIDLabel";
            this.UserIDLabel.Size = new System.Drawing.Size(57, 20);
            this.UserIDLabel.TabIndex = 9;
            this.UserIDLabel.Text = "User ID:";
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordLabel.Location = new System.Drawing.Point(24, 103);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(72, 20);
            this.PasswordLabel.TabIndex = 8;
            this.PasswordLabel.Text = "Password:";
            // 
            // DatabaseLabel
            // 
            this.DatabaseLabel.AutoSize = true;
            this.DatabaseLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DatabaseLabel.Location = new System.Drawing.Point(24, 144);
            this.DatabaseLabel.Name = "DatabaseLabel";
            this.DatabaseLabel.Size = new System.Drawing.Size(69, 20);
            this.DatabaseLabel.TabIndex = 7;
            this.DatabaseLabel.Text = "Database:";
            // 
            // ServerLabel
            // 
            this.ServerLabel.AutoSize = true;
            this.ServerLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerLabel.Location = new System.Drawing.Point(43, 28);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(53, 20);
            this.ServerLabel.TabIndex = 6;
            this.ServerLabel.Text = "Server:";
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(463, 28);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(103, 20);
            this.PortTextBox.TabIndex = 5;
            // 
            // UserIDTextBox
            // 
            this.UserIDTextBox.Location = new System.Drawing.Point(98, 63);
            this.UserIDTextBox.Name = "UserIDTextBox";
            this.UserIDTextBox.Size = new System.Drawing.Size(468, 20);
            this.UserIDTextBox.TabIndex = 4;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(98, 105);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(468, 20);
            this.PasswordTextBox.TabIndex = 3;
            // 
            // DatabaseTextBox
            // 
            this.DatabaseTextBox.Location = new System.Drawing.Point(99, 144);
            this.DatabaseTextBox.Name = "DatabaseTextBox";
            this.DatabaseTextBox.Size = new System.Drawing.Size(468, 20);
            this.DatabaseTextBox.TabIndex = 2;
            // 
            // ServerTextBox
            // 
            this.ServerTextBox.Location = new System.Drawing.Point(99, 30);
            this.ServerTextBox.Name = "ServerTextBox";
            this.ServerTextBox.Size = new System.Drawing.Size(315, 20);
            this.ServerTextBox.TabIndex = 0;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestConnection.Location = new System.Drawing.Point(15, 390);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(94, 44);
            this.btnTestConnection.TabIndex = 1;
            this.btnTestConnection.Text = "Test";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelBtn.Location = new System.Drawing.Point(546, 390);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(94, 44);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "OK";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(320, 390);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(94, 44);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Add";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // SavedConnections
            // 
            this.SavedConnections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SavedConnections.FormattingEnabled = true;
            this.SavedConnections.Location = new System.Drawing.Point(155, 69);
            this.SavedConnections.Name = "SavedConnections";
            this.SavedConnections.Size = new System.Drawing.Size(344, 21);
            this.SavedConnections.TabIndex = 4;
            this.SavedConnections.SelectedIndexChanged += new System.EventHandler(this.SavedConnections_SelectedIndexChanged);
            // 
            // SavedLabel
            // 
            this.SavedLabel.AutoSize = true;
            this.SavedLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SavedLabel.Location = new System.Drawing.Point(12, 67);
            this.SavedLabel.Name = "SavedLabel";
            this.SavedLabel.Size = new System.Drawing.Size(130, 20);
            this.SavedLabel.TabIndex = 15;
            this.SavedLabel.Text = "Saved Connections:";
            // 
            // btnSetDefault
            // 
            this.btnSetDefault.Location = new System.Drawing.Point(505, 65);
            this.btnSetDefault.Name = "btnSetDefault";
            this.btnSetDefault.Size = new System.Drawing.Size(74, 27);
            this.btnSetDefault.TabIndex = 16;
            this.btnSetDefault.Text = "Select";
            this.btnSetDefault.UseVisualStyleBackColor = true;
            this.btnSetDefault.Click += new System.EventHandler(this.btnSetDefault_Click);
            // 
            // SelectedConnection
            // 
            this.SelectedConnection.Location = new System.Drawing.Point(155, 32);
            this.SelectedConnection.Name = "SelectedConnection";
            this.SelectedConnection.ReadOnly = true;
            this.SelectedConnection.Size = new System.Drawing.Size(344, 20);
            this.SelectedConnection.TabIndex = 17;
            this.SelectedConnection.TextChanged += new System.EventHandler(this.SelectedConnection_TextChanged);
            // 
            // SelectedConnectionLabel
            // 
            this.SelectedConnectionLabel.AutoSize = true;
            this.SelectedConnectionLabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectedConnectionLabel.Location = new System.Drawing.Point(11, 30);
            this.SelectedConnectionLabel.Name = "SelectedConnectionLabel";
            this.SelectedConnectionLabel.Size = new System.Drawing.Size(138, 20);
            this.SelectedConnectionLabel.TabIndex = 18;
            this.SelectedConnectionLabel.Text = "Selected Connection:";
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Location = new System.Drawing.Point(585, 65);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(74, 27);
            this.DeleteBtn.TabIndex = 19;
            this.DeleteBtn.Text = "Delete";
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // frmConnectionSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 456);
            this.Controls.Add(this.DeleteBtn);
            this.Controls.Add(this.SelectedConnectionLabel);
            this.Controls.Add(this.SelectedConnection);
            this.Controls.Add(this.btnSetDefault);
            this.Controls.Add(this.SavedLabel);
            this.Controls.Add(this.SavedConnections);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.NewConnectionPanel);
            this.Name = "frmConnectionSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connection Settings";
            this.NewConnectionPanel.ResumeLayout(false);
            this.NewConnectionPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CommandInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel NewConnectionPanel;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.Label UserIDLabel;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.Label DatabaseLabel;
        private System.Windows.Forms.Label ServerLabel;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.TextBox UserIDTextBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.TextBox DatabaseTextBox;
        private System.Windows.Forms.TextBox ServerTextBox;
        private System.Windows.Forms.NumericUpDown CommandInput;
        private System.Windows.Forms.NumericUpDown ConnectInput;
        private System.Windows.Forms.Label CommandLabel;
        private System.Windows.Forms.Label ConnectionLabel;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ComboBox SavedConnections;
        private System.Windows.Forms.Label SavedLabel;
        private System.Windows.Forms.Label TCLabel;
        private System.Windows.Forms.Button btnSetDefault;
        private System.Windows.Forms.CheckBox TrustedConnectionChkBox;
        private System.Windows.Forms.TextBox SelectedConnection;
        private System.Windows.Forms.Label SelectedConnectionLabel;
        private System.Windows.Forms.Button DeleteBtn;
    }
}