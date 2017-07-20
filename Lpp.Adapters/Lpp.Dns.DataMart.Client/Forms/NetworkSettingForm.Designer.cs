using Lpp.Dns.DataMart.Lib;
namespace Lpp.Dns.DataMart.Client
{
    partial class NetworkSettingForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Button btnCancel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NetworkSettingForm));
            this.QuerySourceContainer = new System.Windows.Forms.GroupBox();
            this.tbReceiveTimeout = new System.Windows.Forms.TextBox();
            this.lblStatusMessage = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbCertificates = new System.Windows.Forms.ComboBox();
            this.txtNetWorkId = new System.Windows.Forms.Label();
            this.chkAutoLogin = new System.Windows.Forms.CheckBox();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblServiceUrl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblNetworkId = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtServiceUrl = new System.Windows.Forms.TextBox();
            this.txtNetworkname = new System.Windows.Forms.TextBox();
            this.DataMartsContainer = new System.Windows.Forms.GroupBox();
            this.btnRefreshDMlist = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.dgvDataMarts = new System.Windows.Forms.DataGridView();
            this.DataMartId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DMName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataMartIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataMartNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.organizationIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.organizationNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataSourceNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.allowUnattendedOperationDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pollingIntervalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.processQueriesAndNotUploadDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.processQueriesAndUploadAutomaticallyDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.notifyOfNewQueriesDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.threshHoldCellCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.netWorkSettingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnApply = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.tbHost = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            btnCancel = new System.Windows.Forms.Button();
            this.QuerySourceContainer.SuspendLayout();
            this.DataMartsContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataMarts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.netWorkSettingBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(342, 493);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 42);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // QuerySourceContainer
            // 
            this.QuerySourceContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.QuerySourceContainer.Controls.Add(this.tbReceiveTimeout);
            this.QuerySourceContainer.Controls.Add(this.lblStatusMessage);
            this.QuerySourceContainer.Controls.Add(this.label7);
            this.QuerySourceContainer.Controls.Add(this.cbCertificates);
            this.QuerySourceContainer.Controls.Add(this.txtNetWorkId);
            this.QuerySourceContainer.Controls.Add(this.chkAutoLogin);
            this.QuerySourceContainer.Controls.Add(this.btnTestConnection);
            this.QuerySourceContainer.Controls.Add(this.label1);
            this.QuerySourceContainer.Controls.Add(this.lblPassword);
            this.QuerySourceContainer.Controls.Add(this.label4);
            this.QuerySourceContainer.Controls.Add(this.lblServiceUrl);
            this.QuerySourceContainer.Controls.Add(this.label2);
            this.QuerySourceContainer.Controls.Add(this.lblNetworkId);
            this.QuerySourceContainer.Controls.Add(this.txtPassword);
            this.QuerySourceContainer.Controls.Add(this.txtUsername);
            this.QuerySourceContainer.Controls.Add(this.txtServiceUrl);
            this.QuerySourceContainer.Controls.Add(this.txtNetworkname);
            this.QuerySourceContainer.Location = new System.Drawing.Point(13, 2);
            this.QuerySourceContainer.Name = "QuerySourceContainer";
            this.QuerySourceContainer.Size = new System.Drawing.Size(406, 236);
            this.QuerySourceContainer.TabIndex = 0;
            this.QuerySourceContainer.TabStop = false;
            this.QuerySourceContainer.Text = "Query Source";
            // 
            // tbReceiveTimeout
            // 
            this.tbReceiveTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbReceiveTimeout.Location = new System.Drawing.Point(106, 175);
            this.tbReceiveTimeout.Name = "tbReceiveTimeout";
            this.tbReceiveTimeout.Size = new System.Drawing.Size(30, 20);
            this.tbReceiveTimeout.TabIndex = 17;
            this.tbReceiveTimeout.Text = "120";
            // 
            // lblStatusMessage
            // 
            this.lblStatusMessage.AutoEllipsis = true;
            this.lblStatusMessage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblStatusMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusMessage.ForeColor = System.Drawing.Color.Red;
            this.lblStatusMessage.Location = new System.Drawing.Point(14, 204);
            this.lblStatusMessage.Name = "lblStatusMessage";
            this.lblStatusMessage.Size = new System.Drawing.Size(386, 23);
            this.lblStatusMessage.TabIndex = 3;
            this.lblStatusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(4, 175);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 20);
            this.label7.TabIndex = 16;
            this.label7.Text = "Timeout:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbCertificates
            // 
            this.cbCertificates.DisplayMember = "Name";
            this.cbCertificates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCertificates.FormattingEnabled = true;
            this.cbCertificates.Location = new System.Drawing.Point(108, 149);
            this.cbCertificates.Margin = new System.Windows.Forms.Padding(2);
            this.cbCertificates.Name = "cbCertificates";
            this.cbCertificates.Size = new System.Drawing.Size(287, 21);
            this.cbCertificates.TabIndex = 10;
            // 
            // txtNetWorkId
            // 
            this.txtNetWorkId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNetWorkId.Location = new System.Drawing.Point(108, 20);
            this.txtNetWorkId.Margin = new System.Windows.Forms.Padding(3);
            this.txtNetWorkId.Name = "txtNetWorkId";
            this.txtNetWorkId.Size = new System.Drawing.Size(285, 20);
            this.txtNetWorkId.TabIndex = 13;
            this.txtNetWorkId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkAutoLogin
            // 
            this.chkAutoLogin.AutoSize = true;
            this.chkAutoLogin.Location = new System.Drawing.Point(151, 198);
            this.chkAutoLogin.Name = "chkAutoLogin";
            this.chkAutoLogin.Size = new System.Drawing.Size(117, 17);
            this.chkAutoLogin.TabIndex = 11;
            this.chkAutoLogin.Text = "Login Automatically";
            this.chkAutoLogin.UseVisualStyleBackColor = true;
            this.chkAutoLogin.Visible = false;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestConnection.Location = new System.Drawing.Point(314, 178);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(81, 23);
            this.btnTestConnection.TabIndex = 12;
            this.btnTestConnection.Text = "Test";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(14, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 14);
            this.label1.TabIndex = 9;
            this.label1.Text = "X.509 Certificate:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblPassword
            // 
            this.lblPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPassword.Location = new System.Drawing.Point(14, 127);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(88, 14);
            this.lblPassword.TabIndex = 9;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(14, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 14);
            this.label4.TabIndex = 8;
            this.label4.Text = "Username:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblServiceUrl
            // 
            this.lblServiceUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblServiceUrl.Location = new System.Drawing.Point(13, 75);
            this.lblServiceUrl.Name = "lblServiceUrl";
            this.lblServiceUrl.Size = new System.Drawing.Size(88, 14);
            this.lblServiceUrl.TabIndex = 7;
            this.lblServiceUrl.Text = "Service Url:";
            this.lblServiceUrl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(14, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 14);
            this.label2.TabIndex = 6;
            this.label2.Text = "Network Name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblNetworkId
            // 
            this.lblNetworkId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNetworkId.Location = new System.Drawing.Point(14, 24);
            this.lblNetworkId.Name = "lblNetworkId";
            this.lblNetworkId.Size = new System.Drawing.Size(88, 14);
            this.lblNetworkId.TabIndex = 5;
            this.lblNetworkId.Text = "Network Id:";
            this.lblNetworkId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(108, 124);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(285, 20);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUsername
            // 
            this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsername.Location = new System.Drawing.Point(108, 98);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(285, 20);
            this.txtUsername.TabIndex = 3;
            // 
            // txtServiceUrl
            // 
            this.txtServiceUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServiceUrl.Location = new System.Drawing.Point(108, 72);
            this.txtServiceUrl.Name = "txtServiceUrl";
            this.txtServiceUrl.Size = new System.Drawing.Size(285, 20);
            this.txtServiceUrl.TabIndex = 2;
            this.txtServiceUrl.TextChanged += new System.EventHandler(this.txtServiceUrl_TextChanged);
            this.txtServiceUrl.Leave += new System.EventHandler(this.txtServiceUrl_Leave);
            // 
            // txtNetworkname
            // 
            this.txtNetworkname.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNetworkname.Location = new System.Drawing.Point(108, 46);
            this.txtNetworkname.Name = "txtNetworkname";
            this.txtNetworkname.Size = new System.Drawing.Size(285, 20);
            this.txtNetworkname.TabIndex = 1;
            // 
            // DataMartsContainer
            // 
            this.DataMartsContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataMartsContainer.Controls.Add(this.btnRefreshDMlist);
            this.DataMartsContainer.Controls.Add(this.btnEdit);
            this.DataMartsContainer.Controls.Add(this.dgvDataMarts);
            this.DataMartsContainer.Location = new System.Drawing.Point(13, 244);
            this.DataMartsContainer.Name = "DataMartsContainer";
            this.DataMartsContainer.Size = new System.Drawing.Size(406, 243);
            this.DataMartsContainer.TabIndex = 1;
            this.DataMartsContainer.TabStop = false;
            this.DataMartsContainer.Text = "DataMarts";
            // 
            // btnRefreshDMlist
            // 
            this.btnRefreshDMlist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshDMlist.Location = new System.Drawing.Point(199, 214);
            this.btnRefreshDMlist.Name = "btnRefreshDMlist";
            this.btnRefreshDMlist.Size = new System.Drawing.Size(117, 23);
            this.btnRefreshDMlist.TabIndex = 2;
            this.btnRefreshDMlist.Text = "Refresh DataMarts";
            this.btnRefreshDMlist.UseVisualStyleBackColor = true;
            this.btnRefreshDMlist.Click += new System.EventHandler(this.btnRefreshDMlist_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(321, 214);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(81, 23);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // dgvDataMarts
            // 
            this.dgvDataMarts.AllowUserToAddRows = false;
            this.dgvDataMarts.AllowUserToDeleteRows = false;
            this.dgvDataMarts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDataMarts.AutoGenerateColumns = false;
            this.dgvDataMarts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDataMarts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DataMartId,
            this.DMName,
            this.dataMartIdDataGridViewTextBoxColumn,
            this.dataMartNameDataGridViewTextBoxColumn,
            this.organizationIdDataGridViewTextBoxColumn,
            this.organizationNameDataGridViewTextBoxColumn,
            this.dataSourceNameDataGridViewTextBoxColumn,
            this.allowUnattendedOperationDataGridViewCheckBoxColumn,
            this.pollingIntervalDataGridViewTextBoxColumn,
            this.processQueriesAndNotUploadDataGridViewCheckBoxColumn,
            this.processQueriesAndUploadAutomaticallyDataGridViewCheckBoxColumn,
            this.notifyOfNewQueriesDataGridViewCheckBoxColumn,
            this.threshHoldCellCountDataGridViewTextBoxColumn});
            this.dgvDataMarts.DataMember = "DataMartList";
            this.dgvDataMarts.DataSource = this.netWorkSettingBindingSource;
            this.dgvDataMarts.Location = new System.Drawing.Point(6, 19);
            this.dgvDataMarts.MultiSelect = false;
            this.dgvDataMarts.Name = "dgvDataMarts";
            this.dgvDataMarts.RowHeadersVisible = false;
            this.dgvDataMarts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDataMarts.Size = new System.Drawing.Size(394, 189);
            this.dgvDataMarts.TabIndex = 0;
            this.dgvDataMarts.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDataMarts_CellClick);
            // 
            // DataMartId
            // 
            this.DataMartId.DataPropertyName = "DataMartId";
            this.DataMartId.HeaderText = "DataMart Id";
            this.DataMartId.Name = "DataMartId";
            this.DataMartId.ReadOnly = true;
            // 
            // DMName
            // 
            this.DMName.DataPropertyName = "DataMartName";
            this.DMName.HeaderText = "DataMart Name";
            this.DMName.Name = "DMName";
            this.DMName.ReadOnly = true;
            // 
            // dataMartIdDataGridViewTextBoxColumn
            // 
            this.dataMartIdDataGridViewTextBoxColumn.DataPropertyName = "DataMartId";
            this.dataMartIdDataGridViewTextBoxColumn.HeaderText = "DataMartId";
            this.dataMartIdDataGridViewTextBoxColumn.Name = "dataMartIdDataGridViewTextBoxColumn";
            this.dataMartIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // dataMartNameDataGridViewTextBoxColumn
            // 
            this.dataMartNameDataGridViewTextBoxColumn.DataPropertyName = "DataMartName";
            this.dataMartNameDataGridViewTextBoxColumn.HeaderText = "DataMartName";
            this.dataMartNameDataGridViewTextBoxColumn.Name = "dataMartNameDataGridViewTextBoxColumn";
            this.dataMartNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // organizationIdDataGridViewTextBoxColumn
            // 
            this.organizationIdDataGridViewTextBoxColumn.DataPropertyName = "OrganizationId";
            this.organizationIdDataGridViewTextBoxColumn.HeaderText = "OrganizationId";
            this.organizationIdDataGridViewTextBoxColumn.Name = "organizationIdDataGridViewTextBoxColumn";
            this.organizationIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // organizationNameDataGridViewTextBoxColumn
            // 
            this.organizationNameDataGridViewTextBoxColumn.DataPropertyName = "OrganizationName";
            this.organizationNameDataGridViewTextBoxColumn.HeaderText = "OrganizationName";
            this.organizationNameDataGridViewTextBoxColumn.Name = "organizationNameDataGridViewTextBoxColumn";
            this.organizationNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // dataSourceNameDataGridViewTextBoxColumn
            // 
            this.dataSourceNameDataGridViewTextBoxColumn.DataPropertyName = "DataSourceName";
            this.dataSourceNameDataGridViewTextBoxColumn.HeaderText = "DataSourceName";
            this.dataSourceNameDataGridViewTextBoxColumn.Name = "dataSourceNameDataGridViewTextBoxColumn";
            this.dataSourceNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // allowUnattendedOperationDataGridViewCheckBoxColumn
            // 
            this.allowUnattendedOperationDataGridViewCheckBoxColumn.DataPropertyName = "AllowUnattendedOperation";
            this.allowUnattendedOperationDataGridViewCheckBoxColumn.HeaderText = "AllowUnattendedOperation";
            this.allowUnattendedOperationDataGridViewCheckBoxColumn.Name = "allowUnattendedOperationDataGridViewCheckBoxColumn";
            this.allowUnattendedOperationDataGridViewCheckBoxColumn.Visible = false;
            // 
            // pollingIntervalDataGridViewTextBoxColumn
            // 
            this.pollingIntervalDataGridViewTextBoxColumn.DataPropertyName = "PollingInterval";
            this.pollingIntervalDataGridViewTextBoxColumn.HeaderText = "PollingInterval";
            this.pollingIntervalDataGridViewTextBoxColumn.Name = "pollingIntervalDataGridViewTextBoxColumn";
            this.pollingIntervalDataGridViewTextBoxColumn.Visible = false;
            // 
            // processQueriesAndNotUploadDataGridViewCheckBoxColumn
            // 
            this.processQueriesAndNotUploadDataGridViewCheckBoxColumn.DataPropertyName = "ProcessQueriesAndNotUpload";
            this.processQueriesAndNotUploadDataGridViewCheckBoxColumn.HeaderText = "ProcessQueriesAndNotUpload";
            this.processQueriesAndNotUploadDataGridViewCheckBoxColumn.Name = "processQueriesAndNotUploadDataGridViewCheckBoxColumn";
            this.processQueriesAndNotUploadDataGridViewCheckBoxColumn.Visible = false;
            // 
            // processQueriesAndUploadAutomaticallyDataGridViewCheckBoxColumn
            // 
            this.processQueriesAndUploadAutomaticallyDataGridViewCheckBoxColumn.DataPropertyName = "ProcessQueriesAndUploadAutomatically";
            this.processQueriesAndUploadAutomaticallyDataGridViewCheckBoxColumn.HeaderText = "ProcessQueriesAndUploadAutomatically";
            this.processQueriesAndUploadAutomaticallyDataGridViewCheckBoxColumn.Name = "processQueriesAndUploadAutomaticallyDataGridViewCheckBoxColumn";
            this.processQueriesAndUploadAutomaticallyDataGridViewCheckBoxColumn.Visible = false;
            // 
            // notifyOfNewQueriesDataGridViewCheckBoxColumn
            // 
            this.notifyOfNewQueriesDataGridViewCheckBoxColumn.DataPropertyName = "NotifyOfNewQueries";
            this.notifyOfNewQueriesDataGridViewCheckBoxColumn.HeaderText = "NotifyOfNewQueries";
            this.notifyOfNewQueriesDataGridViewCheckBoxColumn.Name = "notifyOfNewQueriesDataGridViewCheckBoxColumn";
            this.notifyOfNewQueriesDataGridViewCheckBoxColumn.Visible = false;
            // 
            // threshHoldCellCountDataGridViewTextBoxColumn
            // 
            this.threshHoldCellCountDataGridViewTextBoxColumn.DataPropertyName = "ThreshHoldCellCount";
            this.threshHoldCellCountDataGridViewTextBoxColumn.HeaderText = "ThreshHoldCellCount";
            this.threshHoldCellCountDataGridViewTextBoxColumn.Name = "threshHoldCellCountDataGridViewTextBoxColumn";
            this.threshHoldCellCountDataGridViewTextBoxColumn.Visible = false;
            // 
            // netWorkSettingBindingSource
            // 
            this.netWorkSettingBindingSource.DataSource = typeof(Lpp.Dns.DataMart.Lib.NetWorkSetting);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(179, 493);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 42);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(261, 493);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 42);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "SyncRoot";
            this.dataGridViewTextBoxColumn1.HeaderText = "SyncRoot";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 49;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tbPort);
            this.groupBox1.Controls.Add(this.tbHost);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(13, 264);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(406, 53);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SFTP Server";
            this.groupBox1.Visible = false;
            // 
            // tbPort
            // 
            this.tbPort.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPort.Location = new System.Drawing.Point(257, 19);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(132, 20);
            this.tbPort.TabIndex = 9;
            // 
            // tbHost
            // 
            this.tbHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbHost.Location = new System.Drawing.Point(41, 18);
            this.tbHost.Name = "tbHost";
            this.tbHost.Size = new System.Drawing.Size(169, 20);
            this.tbHost.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(223, 22);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Port:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 22);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Host:";
            // 
            // NetworkSettingForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CancelButton = btnCancel;
            this.ClientSize = new System.Drawing.Size(431, 539);
            this.Controls.Add(this.DataMartsContainer);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.QuerySourceContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "NetworkSettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Network Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NetworkSettingForm_Closing);
            this.Load += new System.EventHandler(this.NetworkSettingForm_Load);
            this.QuerySourceContainer.ResumeLayout(false);
            this.QuerySourceContainer.PerformLayout();
            this.DataMartsContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataMarts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.netWorkSettingBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox QuerySourceContainer;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtServiceUrl;
        private System.Windows.Forms.TextBox txtNetworkname;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblServiceUrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblNetworkId;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.GroupBox DataMartsContainer;
        private System.Windows.Forms.DataGridView dgvDataMarts;
        //private System.Windows.Forms.DataGridViewTextBoxColumn DataMartName;
        //private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.BindingSource netWorkSettingBindingSource;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnRefreshDMlist;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox chkAutoLogin;
        private System.Windows.Forms.Label lblStatusMessage;
        private System.Windows.Forms.Label txtNetWorkId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.ComboBox cbCertificates;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataMartId;
        private System.Windows.Forms.DataGridViewTextBoxColumn DMName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataMartIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataMartNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn organizationIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn organizationNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataSourceNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn allowUnattendedOperationDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pollingIntervalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn processQueriesAndNotUploadDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn processQueriesAndUploadAutomaticallyDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn notifyOfNewQueriesDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn threshHoldCellCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.TextBox tbHost;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbReceiveTimeout;
        private System.Windows.Forms.Label label7;        
    }
}