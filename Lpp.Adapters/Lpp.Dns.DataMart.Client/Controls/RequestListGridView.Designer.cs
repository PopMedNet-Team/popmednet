namespace Lpp.Dns.DataMart.Client.Controls
{
    partial class RequestListGridView
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
            System.Windows.Forms.LinkLabel reconnect;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.LinkLabel hideInvisibleRequestsWarning;
            System.Windows.Forms.LinkLabel resetFilters;
            this.dgvRequestList = new System.Windows.Forms.DataGridView();
            this.txtPageNumber = new System.Windows.Forms.TextBox();
            this.btn_Last = new System.Windows.Forms.Button();
            this.btn_Next = new System.Windows.Forms.Button();
            this.btn_Previous = new System.Windows.Forms.Button();
            this.btn_First = new System.Windows.Forms.Button();
            this.cmbPageSize = new System.Windows.Forms.ComboBox();
            this.lblPageSize = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotalPages = new System.Windows.Forms.Label();
            this.cmbDates = new System.Windows.Forms.ComboBox();
            this.lblDates = new System.Windows.Forms.Label();
            this.lblQueryStatus = new System.Windows.Forms.Label();
            this.cmbDataMarts = new System.Windows.Forms.ComboBox();
            this.lblDataMarts = new System.Windows.Forms.Label();
            this.connectionError = new System.Windows.Forms.Panel();
            this.errorMessage = new System.Windows.Forms.Label();
            this.invisibleRequestsWarning = new System.Windows.Forms.Panel();
            this.cmbStatus = new Lpp.Dns.DataMart.Client.Controls.CheckBoxDropDown();
            this.colProject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRequestType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModelType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMSRequestID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPriority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDueDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatedByUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRequestTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataMartName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRespondedByUsername = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResponseTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            reconnect = new System.Windows.Forms.LinkLabel();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            hideInvisibleRequestsWarning = new System.Windows.Forms.LinkLabel();
            resetFilters = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequestList)).BeginInit();
            this.connectionError.SuspendLayout();
            this.invisibleRequestsWarning.SuspendLayout();
            this.SuspendLayout();
            // 
            // reconnect
            // 
            reconnect.Anchor = System.Windows.Forms.AnchorStyles.None;
            reconnect.AutoSize = true;
            reconnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            reconnect.Location = new System.Drawing.Point(295, 145);
            reconnect.Name = "reconnect";
            reconnect.Size = new System.Drawing.Size(163, 24);
            reconnect.TabIndex = 7;
            reconnect.TabStop = true;
            reconnect.Text = "Try to reconnect";
            reconnect.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.reconnect_LinkClicked);
            // 
            // label3
            // 
            label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            label3.Location = new System.Drawing.Point(47, 76);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(584, 20);
            label3.TabIndex = 5;
            label3.Text = "DataMartClient was unable to connection to this network. The error message was:";
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            label2.ForeColor = System.Drawing.Color.Maroon;
            label2.Location = new System.Drawing.Point(296, 45);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(155, 24);
            label2.TabIndex = 6;
            label2.Text = "Network is Offline";
            // 
            // label4
            // 
            label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            label4.Location = new System.Drawing.Point(-1, 8);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(841, 23);
            label4.TabIndex = 0;
            label4.Text = "One or more new requests were submitted on this network, but they are invisible d" +
    "ue to your filter settings.";
            label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // hideInvisibleRequestsWarning
            // 
            hideInvisibleRequestsWarning.Anchor = System.Windows.Forms.AnchorStyles.Top;
            hideInvisibleRequestsWarning.AutoSize = true;
            hideInvisibleRequestsWarning.Location = new System.Drawing.Point(404, 30);
            hideInvisibleRequestsWarning.Name = "hideInvisibleRequestsWarning";
            hideInvisibleRequestsWarning.Size = new System.Drawing.Size(93, 13);
            hideInvisibleRequestsWarning.TabIndex = 2;
            hideInvisibleRequestsWarning.TabStop = true;
            hideInvisibleRequestsWarning.Text = "Hide this message";
            hideInvisibleRequestsWarning.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.hideInvisibleRequestsWarning_LinkClicked);
            // 
            // resetFilters
            // 
            resetFilters.Anchor = System.Windows.Forms.AnchorStyles.Top;
            resetFilters.AutoSize = true;
            resetFilters.Location = new System.Drawing.Point(311, 30);
            resetFilters.Name = "resetFilters";
            resetFilters.Size = new System.Drawing.Size(62, 13);
            resetFilters.TabIndex = 3;
            resetFilters.TabStop = true;
            resetFilters.Text = "Reset filters";
            resetFilters.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.resetFilters_LinkClicked);
            // 
            // dgvRequestList
            // 
            this.dgvRequestList.AllowUserToAddRows = false;
            this.dgvRequestList.AllowUserToDeleteRows = false;
            this.dgvRequestList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRequestList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRequestList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRequestList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colProject,
                this.colRequestType,
                this.colModelType,
                this.colName,
                this.colMSRequestID,
                this.colPriority,
                this.colDueDate,
                this.colStatus,
                this.colCreatedByUserName,
                this.colRequestTime,
                this.colDataMartName,
                this.colRespondedByUsername,
                this.colResponseTime,
                this.colId
            });
            this.dgvRequestList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvRequestList.Location = new System.Drawing.Point(3, 42);
            this.dgvRequestList.MultiSelect = false;
            this.dgvRequestList.Name = "dgvRequestList";
            this.dgvRequestList.ReadOnly = true;
            this.dgvRequestList.RowHeadersVisible = false;
            this.dgvRequestList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRequestList.Size = new System.Drawing.Size(868, 391);
            this.dgvRequestList.TabIndex = 1;
            this.dgvRequestList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRequestList_CellDoubleClick);
            this.dgvRequestList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvRequestList_CellFormatting);
            this.dgvRequestList.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvRequestList_ColumnHeaderMouseClick);
            this.dgvRequestList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvRequestList_KeyDown);
            this.dgvRequestList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvRequestList_KeyUp);
            // 
            // txtPageNumber
            // 
            this.txtPageNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPageNumber.Location = new System.Drawing.Point(712, 442);
            this.txtPageNumber.MaxLength = 4;
            this.txtPageNumber.Name = "txtPageNumber";
            this.txtPageNumber.Size = new System.Drawing.Size(35, 20);
            this.txtPageNumber.TabIndex = 32;
            this.txtPageNumber.Text = "1";
            this.txtPageNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPageNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPageNumber_KeyPress);
            // 
            // btn_Last
            // 
            this.btn_Last.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Last.Location = new System.Drawing.Point(832, 440);
            this.btn_Last.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Last.Name = "btn_Last";
            this.btn_Last.Size = new System.Drawing.Size(35, 25);
            this.btn_Last.TabIndex = 31;
            this.btn_Last.Text = ">|";
            this.btn_Last.UseVisualStyleBackColor = true;
            this.btn_Last.Click += new System.EventHandler(this.btn_Last_Click);
            // 
            // btn_Next
            // 
            this.btn_Next.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Next.Location = new System.Drawing.Point(796, 440);
            this.btn_Next.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Next.Name = "btn_Next";
            this.btn_Next.Size = new System.Drawing.Size(35, 25);
            this.btn_Next.TabIndex = 30;
            this.btn_Next.Text = ">";
            this.btn_Next.UseVisualStyleBackColor = true;
            this.btn_Next.Click += new System.EventHandler(this.btn_Next_Click);
            // 
            // btn_Previous
            // 
            this.btn_Previous.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Previous.Location = new System.Drawing.Point(673, 440);
            this.btn_Previous.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Previous.Name = "btn_Previous";
            this.btn_Previous.Size = new System.Drawing.Size(35, 25);
            this.btn_Previous.TabIndex = 29;
            this.btn_Previous.Text = "<";
            this.btn_Previous.UseVisualStyleBackColor = true;
            this.btn_Previous.Click += new System.EventHandler(this.btn_Previous_Click);
            // 
            // btn_First
            // 
            this.btn_First.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_First.Location = new System.Drawing.Point(637, 440);
            this.btn_First.Margin = new System.Windows.Forms.Padding(2);
            this.btn_First.Name = "btn_First";
            this.btn_First.Size = new System.Drawing.Size(35, 25);
            this.btn_First.TabIndex = 28;
            this.btn_First.Text = "|<";
            this.btn_First.UseVisualStyleBackColor = true;
            this.btn_First.Click += new System.EventHandler(this.btn_First_Click);
            // 
            // cmbPageSize
            // 
            this.cmbPageSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbPageSize.FormattingEnabled = true;
            this.cmbPageSize.Location = new System.Drawing.Point(82, 442);
            this.cmbPageSize.Name = "cmbPageSize";
            this.cmbPageSize.Size = new System.Drawing.Size(56, 21);
            this.cmbPageSize.TabIndex = 33;
            this.cmbPageSize.SelectedIndexChanged += new System.EventHandler(this.cmbPageSize_SelectedIndexChanged);
            this.cmbPageSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbPageSize_KeyPress);
            // 
            // lblPageSize
            // 
            this.lblPageSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPageSize.AutoSize = true;
            this.lblPageSize.Location = new System.Drawing.Point(6, 444);
            this.lblPageSize.Name = "lblPageSize";
            this.lblPageSize.Size = new System.Drawing.Size(56, 13);
            this.lblPageSize.TabIndex = 34;
            this.lblPageSize.Text = "Page size:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(748, 444);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "/";
            // 
            // lblTotalPages
            // 
            this.lblTotalPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalPages.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTotalPages.Location = new System.Drawing.Point(756, 444);
            this.lblTotalPages.Name = "lblTotalPages";
            this.lblTotalPages.Size = new System.Drawing.Size(38, 17);
            this.lblTotalPages.TabIndex = 35;
            this.lblTotalPages.Text = "123";
            this.lblTotalPages.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cmbDates
            // 
            this.cmbDates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDates.Enabled = false;
            this.cmbDates.FormattingEnabled = true;
            this.cmbDates.Items.AddRange(new object[] {
            "30 Days",
            "3 Months",
            "6 Months",
            "1 Year",
            "All",
            "Custom"});
            this.cmbDates.Location = new System.Drawing.Point(630, 8);
            this.cmbDates.Margin = new System.Windows.Forms.Padding(4);
            this.cmbDates.Name = "cmbDates";
            this.cmbDates.Size = new System.Drawing.Size(219, 21);
            this.cmbDates.TabIndex = 42;
            this.cmbDates.SelectedIndexChanged += new System.EventHandler(this.cmbDates_SelectedIndexChanged);
            // 
            // lblDates
            // 
            this.lblDates.AutoSize = true;
            this.lblDates.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDates.Location = new System.Drawing.Point(566, 11);
            this.lblDates.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDates.Name = "lblDates";
            this.lblDates.Size = new System.Drawing.Size(44, 13);
            this.lblDates.TabIndex = 41;
            this.lblDates.Text = "Dates:";
            // 
            // lblQueryStatus
            // 
            this.lblQueryStatus.AutoSize = true;
            this.lblQueryStatus.BackColor = System.Drawing.SystemColors.Control;
            this.lblQueryStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQueryStatus.Location = new System.Drawing.Point(308, 11);
            this.lblQueryStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblQueryStatus.Name = "lblQueryStatus";
            this.lblQueryStatus.Size = new System.Drawing.Size(47, 13);
            this.lblQueryStatus.TabIndex = 40;
            this.lblQueryStatus.Text = "Status:";
            // 
            // cmbDataMarts
            // 
            this.cmbDataMarts.DisplayMember = "DataMartName";
            this.cmbDataMarts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataMarts.Enabled = false;
            this.cmbDataMarts.FormattingEnabled = true;
            this.cmbDataMarts.Location = new System.Drawing.Point(104, 8);
            this.cmbDataMarts.Margin = new System.Windows.Forms.Padding(4);
            this.cmbDataMarts.Name = "cmbDataMarts";
            this.cmbDataMarts.Size = new System.Drawing.Size(188, 21);
            this.cmbDataMarts.TabIndex = 37;
            this.cmbDataMarts.SelectedIndexChanged += new System.EventHandler(this.cmbDataMarts_SelectedIndexChanged);
            // 
            // lblDataMarts
            // 
            this.lblDataMarts.AutoSize = true;
            this.lblDataMarts.BackColor = System.Drawing.SystemColors.Control;
            this.lblDataMarts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataMarts.Location = new System.Drawing.Point(6, 11);
            this.lblDataMarts.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDataMarts.Name = "lblDataMarts";
            this.lblDataMarts.Size = new System.Drawing.Size(69, 13);
            this.lblDataMarts.TabIndex = 38;
            this.lblDataMarts.Text = "DataMarts:";
            // 
            // connectionError
            // 
            this.connectionError.Controls.Add(this.errorMessage);
            this.connectionError.Controls.Add(reconnect);
            this.connectionError.Controls.Add(label3);
            this.connectionError.Controls.Add(label2);
            this.connectionError.Location = new System.Drawing.Point(30, 199);
            this.connectionError.Name = "connectionError";
            this.connectionError.Size = new System.Drawing.Size(801, 210);
            this.connectionError.TabIndex = 45;
            // 
            // errorMessage
            // 
            this.errorMessage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.errorMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.errorMessage.ForeColor = System.Drawing.Color.Red;
            this.errorMessage.Location = new System.Drawing.Point(3, 101);
            this.errorMessage.Name = "errorMessage";
            this.errorMessage.Size = new System.Drawing.Size(795, 44);
            this.errorMessage.TabIndex = 8;
            this.errorMessage.Text = "Error message";
            this.errorMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // invisibleRequestsWarning
            // 
            this.invisibleRequestsWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.invisibleRequestsWarning.BackColor = System.Drawing.Color.AliceBlue;
            this.invisibleRequestsWarning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.invisibleRequestsWarning.Controls.Add(hideInvisibleRequestsWarning);
            this.invisibleRequestsWarning.Controls.Add(resetFilters);
            this.invisibleRequestsWarning.Controls.Add(label4);
            this.invisibleRequestsWarning.Location = new System.Drawing.Point(15, 90);
            this.invisibleRequestsWarning.Name = "invisibleRequestsWarning";
            this.invisibleRequestsWarning.Size = new System.Drawing.Size(841, 55);
            this.invisibleRequestsWarning.TabIndex = 46;
            this.invisibleRequestsWarning.Visible = false;
            // 
            // cmbStatus
            // 
            this.cmbStatus.Enabled = false;
            this.cmbStatus.Location = new System.Drawing.Point(373, 7);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(188, 25);
            this.cmbStatus.TabIndex = 44;
            this.cmbStatus.SelectionChanged += new System.EventHandler(this.cmbStatus_SelectionChanged);
            // 
            // colProject
            // 
            this.colProject.DataPropertyName = "ProjectName";
            this.colProject.HeaderText = "Project";
            this.colProject.Name = "colProject";
            this.colProject.ReadOnly = true;
            this.colProject.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colRequestType
            // 
            this.colRequestType.DataPropertyName = "Type";
            this.colRequestType.HeaderText = "Request Type";
            this.colRequestType.Name = "colRequestType";
            this.colRequestType.ReadOnly = true;
            this.colRequestType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colModelType
            // 
            this.colModelType.DataPropertyName = "ModelName";
            this.colModelType.HeaderText = "Request Model";
            this.colModelType.Name = "colModelType";
            this.colModelType.ReadOnly = true;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Request Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colMSRequestID
            // 
            this.colMSRequestID.DataPropertyName = "MSRequestID";
            this.colMSRequestID.HeaderText = "Request ID";
            this.colMSRequestID.Name = "colMSRequestID";
            this.colMSRequestID.ReadOnly = true;
            this.colMSRequestID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colPriority
            // 
            this.colPriority.DataPropertyName = "Priority";
            this.colPriority.HeaderText = "Priority";
            this.colPriority.Name = "colPriority";
            this.colPriority.ReadOnly = true;
            this.colPriority.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colDueDate
            // 
            this.colDueDate.DataPropertyName = "DueDateNoTime";
            this.colDueDate.HeaderText = "Due Date";
            this.colDueDate.Name = "colDueDate";
            this.colDueDate.ReadOnly = true;
            this.colDueDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "Status";
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colCreatedByUserName
            // 
            this.colCreatedByUserName.DataPropertyName = "CreatedBy";
            this.colCreatedByUserName.HeaderText = "Requestor";
            this.colCreatedByUserName.Name = "colCreatedByUserName";
            this.colCreatedByUserName.ReadOnly = true;
            this.colCreatedByUserName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colRequestTime
            // 
            this.colRequestTime.DataPropertyName = "TimeLocal";
            this.colRequestTime.HeaderText = "Request Time";
            this.colRequestTime.Name = "colRequestTime";
            this.colRequestTime.ReadOnly = true;
            this.colRequestTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colDataMartName
            // 
            this.colDataMartName.DataPropertyName = "DataMartName";
            this.colDataMartName.HeaderText = "DataMart Name";
            this.colDataMartName.Name = "colDataMartName";
            this.colDataMartName.ReadOnly = true;
            this.colDataMartName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colRespondedByUsername
            // 
            this.colRespondedByUsername.DataPropertyName = "RespondedBy";
            this.colRespondedByUsername.HeaderText = "Responder";
            this.colRespondedByUsername.Name = "colRespondedByUsername";
            this.colRespondedByUsername.ReadOnly = true;
            this.colRespondedByUsername.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colResponseTime
            // 
            this.colResponseTime.DataPropertyName = "ResponseTimeLocal";
            this.colResponseTime.HeaderText = "Response Time";
            this.colResponseTime.Name = "colResponseTime";
            this.colResponseTime.ReadOnly = true;
            this.colResponseTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // colId
            // 
            this.colId.DataPropertyName = "Identifier";
            this.colId.HeaderText = "System Number";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // RequestListGridView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.invisibleRequestsWarning);
            this.Controls.Add(this.connectionError);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.cmbDates);
            this.Controls.Add(this.lblDates);
            this.Controls.Add(this.lblQueryStatus);
            this.Controls.Add(this.cmbDataMarts);
            this.Controls.Add(this.lblDataMarts);
            this.Controls.Add(this.lblTotalPages);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblPageSize);
            this.Controls.Add(this.cmbPageSize);
            this.Controls.Add(this.txtPageNumber);
            this.Controls.Add(this.btn_Last);
            this.Controls.Add(this.btn_Next);
            this.Controls.Add(this.btn_Previous);
            this.Controls.Add(this.btn_First);
            this.Controls.Add(this.dgvRequestList);
            this.Name = "RequestListGridView";
            this.Size = new System.Drawing.Size(874, 471);
            this.Load += new System.EventHandler(this.RequestListGridView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequestList)).EndInit();
            this.connectionError.ResumeLayout(false);
            this.connectionError.PerformLayout();
            this.invisibleRequestsWarning.ResumeLayout(false);
            this.invisibleRequestsWarning.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvRequestList;
        private System.Windows.Forms.TextBox txtPageNumber;
        private System.Windows.Forms.Button btn_Last;
        private System.Windows.Forms.Button btn_Next;
        private System.Windows.Forms.Button btn_Previous;
        private System.Windows.Forms.Button btn_First;
        private System.Windows.Forms.ComboBox cmbPageSize;
        private System.Windows.Forms.Label lblPageSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTotalPages;
        private CheckBoxDropDown cmbStatus;
        private System.Windows.Forms.ComboBox cmbDates;
        private System.Windows.Forms.Label lblDates;
        private System.Windows.Forms.Label lblQueryStatus;
        private System.Windows.Forms.ComboBox cmbDataMarts;
        private System.Windows.Forms.Label lblDataMarts;
        private System.Windows.Forms.Panel connectionError;
        private System.Windows.Forms.Label errorMessage;
        private System.Windows.Forms.Panel invisibleRequestsWarning;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRequestType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModelType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMSRequestID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPriority;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDueDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatedByUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRequestTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataMartName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRespondedByUsername;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResponseTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    }
}
