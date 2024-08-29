namespace Display_Security_Settings
{
    partial class UsersOverloadError
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
            this.UOErrorMessage = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // UOErrorMessage
            // 
            this.UOErrorMessage.AutoSize = true;
            this.UOErrorMessage.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UOErrorMessage.Location = new System.Drawing.Point(12, 48);
            this.UOErrorMessage.Name = "UOErrorMessage";
            this.UOErrorMessage.Size = new System.Drawing.Size(383, 21);
            this.UOErrorMessage.TabIndex = 0;
            this.UOErrorMessage.Text = "You have selected too many checkboxes selected.";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(126, 72);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(118, 36);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // UsersOverloadError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 157);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.UOErrorMessage);
            this.Name = "UsersOverloadError";
            this.Text = "Error";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UOErrorMessage;
        private System.Windows.Forms.Button CancelBtn;
    }
}