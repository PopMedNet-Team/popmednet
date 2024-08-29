namespace Display_Security_Settings
{
    partial class NoUsersSelectedError
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
            this.NUSErrorMessage = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NUSErrorMessage
            // 
            this.NUSErrorMessage.AutoSize = true;
            this.NUSErrorMessage.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NUSErrorMessage.Location = new System.Drawing.Point(100, 57);
            this.NUSErrorMessage.Name = "NUSErrorMessage";
            this.NUSErrorMessage.Size = new System.Drawing.Size(217, 21);
            this.NUSErrorMessage.TabIndex = 0;
            this.NUSErrorMessage.Text = "You have no users selected.\r\n";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(154, 81);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(99, 37);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // NoUsersSelectedError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 154);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.NUSErrorMessage);
            this.Name = "NoUsersSelectedError";
            this.Text = "NoUsersSelectedError";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NUSErrorMessage;
        private System.Windows.Forms.Button CancelBtn;
    }
}