using Lpp.Dns.DataMart.Lib.Classes;
namespace Lpp.Dns.DataMart.Client
{
    partial class RequestDetailForm
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
            if (disposing)
            {
                if(components != null)
                    components.Dispose();

                if (_domainManager != null)
                {
                    Processor = null;
                    _domainManager.Dispose();
                    _domainManager = null;
                }

                if(_patIDprogressForm != null)
                {
                    _patIDprogressForm.Dispose();
                    _patIDprogressForm = null;
                }
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
            System.Windows.Forms.Label queryIdLabel;
            System.Windows.Forms.Label requestTimeLabel;
            System.Windows.Forms.Label queryCreatedByUserNameLabel;
            System.Windows.Forms.Label queryStatusTypeIdLabel;
            System.Windows.Forms.Label queryNameLabel;
            System.Windows.Forms.Label queryDescriptionLabel;
            System.Windows.Forms.Label requestorEmailLabel;
            System.Windows.Forms.Label lblNetwork;
            System.Windows.Forms.Label lblDataMart;
            System.Windows.Forms.Button btn_Close;
            System.Windows.Forms.Label lblAdditionalInstructions;
            System.Windows.Forms.Label lblSubmittedByNote;
            System.Windows.Forms.Label lblSubmittedTo;
            System.Windows.Forms.Label activityDueDateLabel;
            System.Windows.Forms.Label activityLabel;
            System.Windows.Forms.Label queryActivityPriorityLabel;
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RequestDetailForm));
            this.queryDataMartBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.queryIdTextBox = new System.Windows.Forms.TextBox();
            this.queryCreatedByUserNameTextBox = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnUploadResults = new System.Windows.Forms.Button();
            this.btnRejectQuery = new System.Windows.Forms.Button();
            this.queryStatusTypeIdTextBox = new System.Windows.Forms.TextBox();
            this.queryNameTextBox = new System.Windows.Forms.TextBox();
            this.requestorEmailTextBox = new System.Windows.Forms.TextBox();
            this.btnHold = new System.Windows.Forms.Button();
            this.txtNetwork = new System.Windows.Forms.TextBox();
            this.txtDataMart = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnExportResults = new System.Windows.Forms.Button();
            this.activityDueDateTextbox = new System.Windows.Forms.TextBox();
            this.activityTextbox = new System.Windows.Forms.TextBox();
            this.queryActivityPriorityTextbox = new System.Windows.Forms.TextBox();
            this.btnDeleteFile = new System.Windows.Forms.Button();
            this.btnAddFile = new System.Windows.Forms.Button();
            this.FileDialog = new System.Windows.Forms.OpenFileDialog();
            this.processRequestWorker = new System.ComponentModel.BackgroundWorker();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lblRequest = new System.Windows.Forms.Label();
            this.lblResponse = new System.Windows.Forms.Label();
            this.cbRequestFileList = new System.Windows.Forms.CheckBox();
            this.cbResponseFileList = new System.Windows.Forms.CheckBox();
            this.btnPostProcess = new System.Windows.Forms.Button();
            this.txtPurposeOfUse = new System.Windows.Forms.TextBox();
            this.lblPurposeOfUse = new System.Windows.Forms.Label();
            this.lblPhiDisclosureLevel = new System.Windows.Forms.Label();
            this.lblTaskOrder = new System.Windows.Forms.Label();
            this.lblActivity = new System.Windows.Forms.Label();
            this.lblActivityProject = new System.Windows.Forms.Label();
            this.txtPhiDisclosureLevel = new System.Windows.Forms.TextBox();
            this.txtTaskOrder = new System.Windows.Forms.TextBox();
            this.txtActivityProject = new System.Windows.Forms.TextBox();
            this.txtSourceActivity = new System.Windows.Forms.TextBox();
            this.lblRequesterCenter = new System.Windows.Forms.Label();
            this.txtRequestorCenter = new System.Windows.Forms.TextBox();
            this.txtWorkplanType = new System.Windows.Forms.TextBox();
            this.lblWorkplanType = new System.Windows.Forms.Label();
            this.txtAdditionalInstructions = new System.Windows.Forms.TextBox();
            this.btnViewSQL = new System.Windows.Forms.Button();
            this.queryMSRequestIDTextBox = new System.Windows.Forms.TextBox();
            this.queryMSRequestIDLabel = new System.Windows.Forms.Label();
            this.txtSourceTaskOrder = new System.Windows.Forms.TextBox();
            this.txtActivity = new System.Windows.Forms.TextBox();
            this.txtSourceActivityProject = new System.Windows.Forms.TextBox();
            this.lblSourceTaskOrder = new System.Windows.Forms.Label();
            this.lblSourceActivity = new System.Windows.Forms.Label();
            this.lblSourceActivityProject = new System.Windows.Forms.Label();
            this.descriptionBrowser = new System.Windows.Forms.WebBrowser();
            this.txtReportAggregationLevel = new System.Windows.Forms.TextBox();
            this.lblReportAggregationLevel = new System.Windows.Forms.Label();
            this.browserPanel = new System.Windows.Forms.Panel();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.vpRequest = new Lpp.Dns.DataMart.Client.Controls.DataMartViewPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.vpResponse = new Lpp.Dns.DataMart.Client.Controls.DataMartViewPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtRequestTimeLocal = new System.Windows.Forms.TextBox();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnClearCache = new System.Windows.Forms.Button();
            this.chkGeneratePATIDList = new System.Windows.Forms.CheckBox();
            queryIdLabel = new System.Windows.Forms.Label();
            requestTimeLabel = new System.Windows.Forms.Label();
            queryCreatedByUserNameLabel = new System.Windows.Forms.Label();
            queryStatusTypeIdLabel = new System.Windows.Forms.Label();
            queryNameLabel = new System.Windows.Forms.Label();
            queryDescriptionLabel = new System.Windows.Forms.Label();
            requestorEmailLabel = new System.Windows.Forms.Label();
            lblNetwork = new System.Windows.Forms.Label();
            lblDataMart = new System.Windows.Forms.Label();
            btn_Close = new System.Windows.Forms.Button();
            lblAdditionalInstructions = new System.Windows.Forms.Label();
            lblSubmittedByNote = new System.Windows.Forms.Label();
            lblSubmittedTo = new System.Windows.Forms.Label();
            activityDueDateLabel = new System.Windows.Forms.Label();
            activityLabel = new System.Windows.Forms.Label();
            queryActivityPriorityLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.queryDataMartBindingSource)).BeginInit();
            this.browserPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // queryIdLabel
            // 
            queryIdLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            queryIdLabel.AutoSize = true;
            queryIdLabel.Location = new System.Drawing.Point(756, 29);
            queryIdLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryIdLabel.Name = "queryIdLabel";
            queryIdLabel.Size = new System.Drawing.Size(84, 13);
            queryIdLabel.TabIndex = 40;
            queryIdLabel.Text = "System Number:";
            // 
            // requestTimeLabel
            // 
            requestTimeLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            requestTimeLabel.AutoSize = true;
            requestTimeLabel.Location = new System.Drawing.Point(373, 77);
            requestTimeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            requestTimeLabel.Name = "requestTimeLabel";
            requestTimeLabel.Size = new System.Drawing.Size(76, 13);
            requestTimeLabel.TabIndex = 39;
            requestTimeLabel.Text = "Request Time:";
            // 
            // queryCreatedByUserNameLabel
            // 
            queryCreatedByUserNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            queryCreatedByUserNameLabel.AutoSize = true;
            queryCreatedByUserNameLabel.Location = new System.Drawing.Point(59, 77);
            queryCreatedByUserNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryCreatedByUserNameLabel.Name = "queryCreatedByUserNameLabel";
            queryCreatedByUserNameLabel.Size = new System.Drawing.Size(59, 13);
            queryCreatedByUserNameLabel.TabIndex = 38;
            queryCreatedByUserNameLabel.Text = "Requestor:";
            // 
            // queryStatusTypeIdLabel
            // 
            queryStatusTypeIdLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            queryStatusTypeIdLabel.AutoSize = true;
            queryStatusTypeIdLabel.Location = new System.Drawing.Point(800, 53);
            queryStatusTypeIdLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryStatusTypeIdLabel.Name = "queryStatusTypeIdLabel";
            queryStatusTypeIdLabel.Size = new System.Drawing.Size(40, 13);
            queryStatusTypeIdLabel.TabIndex = 40;
            queryStatusTypeIdLabel.Text = "Status:";
            // 
            // queryNameLabel
            // 
            queryNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            queryNameLabel.AutoSize = true;
            queryNameLabel.Location = new System.Drawing.Point(37, 29);
            queryNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryNameLabel.Name = "queryNameLabel";
            queryNameLabel.Size = new System.Drawing.Size(81, 13);
            queryNameLabel.TabIndex = 37;
            queryNameLabel.Text = "Request Name:";
            // 
            // queryDescriptionLabel
            // 
            queryDescriptionLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            queryDescriptionLabel.AutoSize = true;
            queryDescriptionLabel.Location = new System.Drawing.Point(2, 3);
            queryDescriptionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryDescriptionLabel.Name = "queryDescriptionLabel";
            queryDescriptionLabel.Size = new System.Drawing.Size(63, 13);
            queryDescriptionLabel.TabIndex = 41;
            queryDescriptionLabel.Text = "Description:";
            // 
            // requestorEmailLabel
            // 
            requestorEmailLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            requestorEmailLabel.AutoSize = true;
            requestorEmailLabel.Location = new System.Drawing.Point(805, 77);
            requestorEmailLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            requestorEmailLabel.Name = "requestorEmailLabel";
            requestorEmailLabel.Size = new System.Drawing.Size(35, 13);
            requestorEmailLabel.TabIndex = 40;
            requestorEmailLabel.Text = "Email:";
            // 
            // lblNetwork
            // 
            lblNetwork.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblNetwork.AutoSize = true;
            lblNetwork.Location = new System.Drawing.Point(68, 5);
            lblNetwork.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblNetwork.Name = "lblNetwork";
            lblNetwork.Size = new System.Drawing.Size(50, 13);
            lblNetwork.TabIndex = 40;
            lblNetwork.Text = "Network:";
            // 
            // lblDataMart
            // 
            lblDataMart.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblDataMart.AutoSize = true;
            lblDataMart.Location = new System.Drawing.Point(786, 5);
            lblDataMart.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblDataMart.Name = "lblDataMart";
            lblDataMart.Size = new System.Drawing.Size(54, 13);
            lblDataMart.TabIndex = 40;
            lblDataMart.Text = "DataMart:";
            // 
            // btn_Close
            // 
            btn_Close.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btn_Close.Location = new System.Drawing.Point(998, 6);
            btn_Close.Margin = new System.Windows.Forms.Padding(2);
            btn_Close.Name = "btn_Close";
            btn_Close.Size = new System.Drawing.Size(80, 24);
            btn_Close.TabIndex = 36;
            btn_Close.Text = "Close";
            btn_Close.UseVisualStyleBackColor = true;
            btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // lblAdditionalInstructions
            // 
            lblAdditionalInstructions.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblAdditionalInstructions.AutoSize = true;
            lblAdditionalInstructions.Location = new System.Drawing.Point(4, 3);
            lblAdditionalInstructions.Margin = new System.Windows.Forms.Padding(4, 0, 3, 0);
            lblAdditionalInstructions.Name = "lblAdditionalInstructions";
            lblAdditionalInstructions.Size = new System.Drawing.Size(113, 13);
            lblAdditionalInstructions.TabIndex = 61;
            lblAdditionalInstructions.Text = "Additional Instructions:";
            // 
            // lblSubmittedByNote
            // 
            lblSubmittedByNote.AutoSize = true;
            lblSubmittedByNote.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "CreatedByMessage", true));
            lblSubmittedByNote.Location = new System.Drawing.Point(90, 205);
            lblSubmittedByNote.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblSubmittedByNote.Name = "lblSubmittedByNote";
            lblSubmittedByNote.Size = new System.Drawing.Size(0, 13);
            lblSubmittedByNote.TabIndex = 62;
            lblSubmittedByNote.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // queryDataMartBindingSource
            // 
            this.queryDataMartBindingSource.DataSource = typeof(Lpp.Dns.DataMart.Lib.Classes.HubRequest);
            // 
            // lblSubmittedTo
            // 
            lblSubmittedTo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            lblSubmittedTo.AutoSize = true;
            lblSubmittedTo.Location = new System.Drawing.Point(45, 198);
            lblSubmittedTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblSubmittedTo.Name = "lblSubmittedTo";
            lblSubmittedTo.Size = new System.Drawing.Size(73, 13);
            lblSubmittedTo.TabIndex = 65;
            lblSubmittedTo.Text = "Submitted To:";
            // 
            // activityDueDateLabel
            // 
            activityDueDateLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            activityDueDateLabel.AutoSize = true;
            activityDueDateLabel.Location = new System.Drawing.Point(393, 53);
            activityDueDateLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            activityDueDateLabel.Name = "activityDueDateLabel";
            activityDueDateLabel.Size = new System.Drawing.Size(56, 13);
            activityDueDateLabel.TabIndex = 72;
            activityDueDateLabel.Text = "Due Date:";
            // 
            // activityLabel
            // 
            activityLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            activityLabel.AutoSize = true;
            activityLabel.Location = new System.Drawing.Point(406, 5);
            activityLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            activityLabel.Name = "activityLabel";
            activityLabel.Size = new System.Drawing.Size(43, 13);
            activityLabel.TabIndex = 70;
            activityLabel.Text = "Project:";
            // 
            // queryActivityPriorityLabel
            // 
            queryActivityPriorityLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            queryActivityPriorityLabel.AutoSize = true;
            queryActivityPriorityLabel.Location = new System.Drawing.Point(77, 53);
            queryActivityPriorityLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            queryActivityPriorityLabel.Name = "queryActivityPriorityLabel";
            queryActivityPriorityLabel.Size = new System.Drawing.Size(41, 13);
            queryActivityPriorityLabel.TabIndex = 68;
            queryActivityPriorityLabel.Text = "Priority:";
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(372, 29);
            label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(77, 13);
            label1.TabIndex = 80;
            label1.Text = "Request Type:";
            // 
            // queryIdTextBox
            // 
            this.queryIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.queryIdTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Identifier", true));
            this.queryIdTextBox.Location = new System.Drawing.Point(845, 27);
            this.queryIdTextBox.Name = "queryIdTextBox";
            this.queryIdTextBox.ReadOnly = true;
            this.queryIdTextBox.Size = new System.Drawing.Size(226, 20);
            this.queryIdTextBox.TabIndex = 5;
            // 
            // queryCreatedByUserNameTextBox
            // 
            this.queryCreatedByUserNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.queryCreatedByUserNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Author.Username", true));
            this.queryCreatedByUserNameTextBox.Location = new System.Drawing.Point(123, 75);
            this.queryCreatedByUserNameTextBox.Name = "queryCreatedByUserNameTextBox";
            this.queryCreatedByUserNameTextBox.ReadOnly = true;
            this.queryCreatedByUserNameTextBox.Size = new System.Drawing.Size(225, 20);
            this.queryCreatedByUserNameTextBox.TabIndex = 9;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRun.Location = new System.Drawing.Point(8, 6);
            this.btnRun.Margin = new System.Windows.Forms.Padding(2);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(80, 24);
            this.btnRun.TabIndex = 27;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnUploadResults
            // 
            this.btnUploadResults.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnUploadResults.Location = new System.Drawing.Point(889, 6);
            this.btnUploadResults.Margin = new System.Windows.Forms.Padding(2);
            this.btnUploadResults.Name = "btnUploadResults";
            this.btnUploadResults.Size = new System.Drawing.Size(100, 24);
            this.btnUploadResults.TabIndex = 35;
            this.btnUploadResults.Text = "Upload Results";
            this.btnUploadResults.UseVisualStyleBackColor = true;
            this.btnUploadResults.Click += new System.EventHandler(this.btnUploadResults_Click);
            // 
            // btnRejectQuery
            // 
            this.btnRejectQuery.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRejectQuery.Location = new System.Drawing.Point(186, 6);
            this.btnRejectQuery.Margin = new System.Windows.Forms.Padding(2);
            this.btnRejectQuery.Name = "btnRejectQuery";
            this.btnRejectQuery.Size = new System.Drawing.Size(80, 24);
            this.btnRejectQuery.TabIndex = 29;
            this.btnRejectQuery.Text = "Reject";
            this.btnRejectQuery.UseVisualStyleBackColor = true;
            this.btnRejectQuery.Click += new System.EventHandler(this.btnRejectQuery_Click);
            // 
            // queryStatusTypeIdTextBox
            // 
            this.queryStatusTypeIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.queryStatusTypeIdTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "RoutingStatus", true));
            this.queryStatusTypeIdTextBox.Location = new System.Drawing.Point(845, 51);
            this.queryStatusTypeIdTextBox.Name = "queryStatusTypeIdTextBox";
            this.queryStatusTypeIdTextBox.ReadOnly = true;
            this.queryStatusTypeIdTextBox.Size = new System.Drawing.Size(226, 20);
            this.queryStatusTypeIdTextBox.TabIndex = 8;
            // 
            // queryNameTextBox
            // 
            this.queryNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.queryNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Name", true));
            this.queryNameTextBox.Location = new System.Drawing.Point(123, 27);
            this.queryNameTextBox.Name = "queryNameTextBox";
            this.queryNameTextBox.ReadOnly = true;
            this.queryNameTextBox.Size = new System.Drawing.Size(225, 20);
            this.queryNameTextBox.TabIndex = 3;
            // 
            // requestorEmailTextBox
            // 
            this.requestorEmailTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.requestorEmailTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Author.Email", true));
            this.requestorEmailTextBox.Location = new System.Drawing.Point(845, 75);
            this.requestorEmailTextBox.Name = "requestorEmailTextBox";
            this.requestorEmailTextBox.ReadOnly = true;
            this.requestorEmailTextBox.Size = new System.Drawing.Size(226, 20);
            this.requestorEmailTextBox.TabIndex = 11;
            // 
            // btnHold
            // 
            this.btnHold.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnHold.Location = new System.Drawing.Point(97, 6);
            this.btnHold.Margin = new System.Windows.Forms.Padding(2);
            this.btnHold.Name = "btnHold";
            this.btnHold.Size = new System.Drawing.Size(80, 24);
            this.btnHold.TabIndex = 28;
            this.btnHold.Text = "Hold";
            this.btnHold.UseVisualStyleBackColor = true;
            this.btnHold.Click += new System.EventHandler(this.btnHold_Click);
            // 
            // txtNetwork
            // 
            this.txtNetwork.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNetwork.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "NetworkName", true));
            this.txtNetwork.Location = new System.Drawing.Point(123, 3);
            this.txtNetwork.Name = "txtNetwork";
            this.txtNetwork.ReadOnly = true;
            this.txtNetwork.Size = new System.Drawing.Size(225, 20);
            this.txtNetwork.TabIndex = 0;
            // 
            // txtDataMart
            // 
            this.txtDataMart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDataMart.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "DataMartName", true));
            this.txtDataMart.Location = new System.Drawing.Point(845, 3);
            this.txtDataMart.Name = "txtDataMart";
            this.txtDataMart.ReadOnly = true;
            this.txtDataMart.Size = new System.Drawing.Size(226, 20);
            this.txtDataMart.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.textBox1, 5);
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "SubmittedDataMarts", true));
            this.textBox1.Location = new System.Drawing.Point(123, 195);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(948, 20);
            this.textBox1.TabIndex = 20;
            // 
            // btnExportResults
            // 
            this.btnExportResults.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnExportResults.Location = new System.Drawing.Point(780, 6);
            this.btnExportResults.Margin = new System.Windows.Forms.Padding(2);
            this.btnExportResults.Name = "btnExportResults";
            this.btnExportResults.Size = new System.Drawing.Size(100, 24);
            this.btnExportResults.TabIndex = 34;
            this.btnExportResults.Text = "Export Results..";
            this.btnExportResults.UseVisualStyleBackColor = true;
            this.btnExportResults.Click += new System.EventHandler(this.btnExportResults_Click);
            // 
            // activityDueDateTextbox
            // 
            this.activityDueDateTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.activityDueDateTextbox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.DueDateNoTime", true));
            this.activityDueDateTextbox.Location = new System.Drawing.Point(454, 51);
            this.activityDueDateTextbox.Name = "activityDueDateTextbox";
            this.activityDueDateTextbox.ReadOnly = true;
            this.activityDueDateTextbox.Size = new System.Drawing.Size(225, 20);
            this.activityDueDateTextbox.TabIndex = 7;
            // 
            // activityTextbox
            // 
            this.activityTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.activityTextbox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "ProjectName", true));
            this.activityTextbox.Location = new System.Drawing.Point(454, 3);
            this.activityTextbox.Name = "activityTextbox";
            this.activityTextbox.ReadOnly = true;
            this.activityTextbox.Size = new System.Drawing.Size(225, 20);
            this.activityTextbox.TabIndex = 1;
            // 
            // queryActivityPriorityTextbox
            // 
            this.queryActivityPriorityTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.queryActivityPriorityTextbox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Priority", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.queryActivityPriorityTextbox.Location = new System.Drawing.Point(123, 51);
            this.queryActivityPriorityTextbox.Name = "queryActivityPriorityTextbox";
            this.queryActivityPriorityTextbox.ReadOnly = true;
            this.queryActivityPriorityTextbox.Size = new System.Drawing.Size(225, 20);
            this.queryActivityPriorityTextbox.TabIndex = 6;
            // 
            // btnDeleteFile
            // 
            this.btnDeleteFile.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDeleteFile.Location = new System.Drawing.Point(453, 6);
            this.btnDeleteFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteFile.Name = "btnDeleteFile";
            this.btnDeleteFile.Size = new System.Drawing.Size(80, 24);
            this.btnDeleteFile.TabIndex = 32;
            this.btnDeleteFile.Text = "Delete File";
            this.btnDeleteFile.UseVisualStyleBackColor = true;
            this.btnDeleteFile.Click += new System.EventHandler(this.btnDeleteFile_Click);
            // 
            // btnAddFile
            // 
            this.btnAddFile.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddFile.Location = new System.Drawing.Point(364, 6);
            this.btnAddFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddFile.Name = "btnAddFile";
            this.btnAddFile.Size = new System.Drawing.Size(80, 24);
            this.btnAddFile.TabIndex = 31;
            this.btnAddFile.Text = "Add File";
            this.btnAddFile.UseVisualStyleBackColor = true;
            this.btnAddFile.Click += new System.EventHandler(this.btnAddFile_Click);
            // 
            // FileDialog
            // 
            this.FileDialog.FileName = "openFileDialog1";
            // 
            // processRequestWorker
            // 
            this.processRequestWorker.WorkerReportsProgress = true;
            this.processRequestWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.processRequestWorker_ProcessRequest);
            this.processRequestWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.processRequestWorker_ProgressChanged);
            this.processRequestWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.processRequestWorker_RunWorkerCompleted);
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.RequestTypeName", true));
            this.textBox2.Location = new System.Drawing.Point(454, 27);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(225, 20);
            this.textBox2.TabIndex = 4;
            // 
            // lblRequest
            // 
            this.lblRequest.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblRequest.AutoSize = true;
            this.lblRequest.Location = new System.Drawing.Point(4, 6);
            this.lblRequest.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRequest.Name = "lblRequest";
            this.lblRequest.Size = new System.Drawing.Size(50, 13);
            this.lblRequest.TabIndex = 82;
            this.lblRequest.Tag = "";
            this.lblRequest.Text = "Request:";
            // 
            // lblResponse
            // 
            this.lblResponse.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblResponse.AutoSize = true;
            this.lblResponse.Location = new System.Drawing.Point(4, 6);
            this.lblResponse.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(58, 13);
            this.lblResponse.TabIndex = 83;
            this.lblResponse.Tag = "Request";
            this.lblResponse.Text = "Response:";
            // 
            // cbRequestFileList
            // 
            this.cbRequestFileList.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cbRequestFileList.AutoSize = true;
            this.cbRequestFileList.Location = new System.Drawing.Point(966, 4);
            this.cbRequestFileList.Margin = new System.Windows.Forms.Padding(4);
            this.cbRequestFileList.Name = "cbRequestFileList";
            this.cbRequestFileList.Size = new System.Drawing.Size(104, 17);
            this.cbRequestFileList.TabIndex = 24;
            this.cbRequestFileList.Text = "View Raw File(s)";
            this.cbRequestFileList.UseVisualStyleBackColor = true;
            // 
            // cbResponseFileList
            // 
            this.cbResponseFileList.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cbResponseFileList.AutoSize = true;
            this.cbResponseFileList.Location = new System.Drawing.Point(966, 4);
            this.cbResponseFileList.Margin = new System.Windows.Forms.Padding(4);
            this.cbResponseFileList.Name = "cbResponseFileList";
            this.cbResponseFileList.Size = new System.Drawing.Size(104, 17);
            this.cbResponseFileList.TabIndex = 26;
            this.cbResponseFileList.Text = "View Raw File(s)";
            this.cbResponseFileList.UseVisualStyleBackColor = true;
            // 
            // btnPostProcess
            // 
            this.btnPostProcess.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPostProcess.Enabled = false;
            this.btnPostProcess.Location = new System.Drawing.Point(542, 6);
            this.btnPostProcess.Margin = new System.Windows.Forms.Padding(4);
            this.btnPostProcess.Name = "btnPostProcess";
            this.btnPostProcess.Size = new System.Drawing.Size(110, 24);
            this.btnPostProcess.TabIndex = 33;
            this.btnPostProcess.Text = "Suppress Low Cells";
            this.btnPostProcess.UseVisualStyleBackColor = true;
            this.btnPostProcess.Click += new System.EventHandler(this.btnPostProcess_Click);
            // 
            // txtPurposeOfUse
            // 
            this.txtPurposeOfUse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPurposeOfUse.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.PurposeOfUse", true));
            this.txtPurposeOfUse.Location = new System.Drawing.Point(123, 99);
            this.txtPurposeOfUse.Name = "txtPurposeOfUse";
            this.txtPurposeOfUse.ReadOnly = true;
            this.txtPurposeOfUse.Size = new System.Drawing.Size(225, 20);
            this.txtPurposeOfUse.TabIndex = 12;
            // 
            // lblPurposeOfUse
            // 
            this.lblPurposeOfUse.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPurposeOfUse.AutoSize = true;
            this.lblPurposeOfUse.Location = new System.Drawing.Point(33, 101);
            this.lblPurposeOfUse.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPurposeOfUse.Name = "lblPurposeOfUse";
            this.lblPurposeOfUse.Size = new System.Drawing.Size(83, 13);
            this.lblPurposeOfUse.TabIndex = 88;
            this.lblPurposeOfUse.Text = "Purpose of Use:";
            // 
            // lblPhiDisclosureLevel
            // 
            this.lblPhiDisclosureLevel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPhiDisclosureLevel.AutoSize = true;
            this.lblPhiDisclosureLevel.Location = new System.Drawing.Point(378, 101);
            this.lblPhiDisclosureLevel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPhiDisclosureLevel.Name = "lblPhiDisclosureLevel";
            this.lblPhiDisclosureLevel.Size = new System.Drawing.Size(69, 13);
            this.lblPhiDisclosureLevel.TabIndex = 89;
            this.lblPhiDisclosureLevel.Text = "Level of PHI:";
            // 
            // lblTaskOrder
            // 
            this.lblTaskOrder.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTaskOrder.AutoSize = true;
            this.lblTaskOrder.Location = new System.Drawing.Point(16, 149);
            this.lblTaskOrder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTaskOrder.Name = "lblTaskOrder";
            this.lblTaskOrder.Size = new System.Drawing.Size(100, 13);
            this.lblTaskOrder.TabIndex = 90;
            this.lblTaskOrder.Text = "Budget Task Order:";
            // 
            // lblActivity
            // 
            this.lblActivity.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblActivity.AutoSize = true;
            this.lblActivity.Location = new System.Drawing.Point(366, 149);
            this.lblActivity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblActivity.Name = "lblActivity";
            this.lblActivity.Size = new System.Drawing.Size(81, 13);
            this.lblActivity.TabIndex = 91;
            this.lblActivity.Text = "Budget Activity:";
            // 
            // lblActivityProject
            // 
            this.lblActivityProject.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblActivityProject.AutoSize = true;
            this.lblActivityProject.Location = new System.Drawing.Point(721, 149);
            this.lblActivityProject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblActivityProject.Name = "lblActivityProject";
            this.lblActivityProject.Size = new System.Drawing.Size(117, 13);
            this.lblActivityProject.TabIndex = 92;
            this.lblActivityProject.Text = "Budget Activity Project:";
            // 
            // txtPhiDisclosureLevel
            // 
            this.txtPhiDisclosureLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPhiDisclosureLevel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.PhiDisclosureLevel", true));
            this.txtPhiDisclosureLevel.Location = new System.Drawing.Point(454, 99);
            this.txtPhiDisclosureLevel.Name = "txtPhiDisclosureLevel";
            this.txtPhiDisclosureLevel.ReadOnly = true;
            this.txtPhiDisclosureLevel.Size = new System.Drawing.Size(225, 20);
            this.txtPhiDisclosureLevel.TabIndex = 13;
            // 
            // txtTaskOrder
            // 
            this.txtTaskOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTaskOrder.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.TaskOrder", true));
            this.txtTaskOrder.Location = new System.Drawing.Point(123, 147);
            this.txtTaskOrder.Name = "txtTaskOrder";
            this.txtTaskOrder.ReadOnly = true;
            this.txtTaskOrder.Size = new System.Drawing.Size(225, 20);
            this.txtTaskOrder.TabIndex = 15;
            // 
            // txtActivityProject
            // 
            this.txtActivityProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtActivityProject.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.ActivityProject", true));
            this.txtActivityProject.Location = new System.Drawing.Point(845, 147);
            this.txtActivityProject.Name = "txtActivityProject";
            this.txtActivityProject.ReadOnly = true;
            this.txtActivityProject.Size = new System.Drawing.Size(226, 20);
            this.txtActivityProject.TabIndex = 17;
            // 
            // txtSourceActivity
            // 
            this.txtSourceActivity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceActivity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.SourceActivity", true));
            this.txtSourceActivity.Location = new System.Drawing.Point(454, 123);
            this.txtSourceActivity.Name = "txtSourceActivity";
            this.txtSourceActivity.ReadOnly = true;
            this.txtSourceActivity.Size = new System.Drawing.Size(225, 20);
            this.txtSourceActivity.TabIndex = 16;
            // 
            // lblRequesterCenter
            // 
            this.lblRequesterCenter.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblRequesterCenter.AutoSize = true;
            this.lblRequesterCenter.Location = new System.Drawing.Point(23, 173);
            this.lblRequesterCenter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRequesterCenter.Name = "lblRequesterCenter";
            this.lblRequesterCenter.Size = new System.Drawing.Size(93, 13);
            this.lblRequesterCenter.TabIndex = 97;
            this.lblRequesterCenter.Text = "Requester Center:";
            // 
            // txtRequestorCenter
            // 
            this.txtRequestorCenter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRequestorCenter.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.RequestorCenter", true));
            this.txtRequestorCenter.Location = new System.Drawing.Point(123, 171);
            this.txtRequestorCenter.Name = "txtRequestorCenter";
            this.txtRequestorCenter.ReadOnly = true;
            this.txtRequestorCenter.Size = new System.Drawing.Size(225, 20);
            this.txtRequestorCenter.TabIndex = 18;
            // 
            // txtWorkplanType
            // 
            this.txtWorkplanType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWorkplanType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.WorkplanType", true));
            this.txtWorkplanType.Location = new System.Drawing.Point(454, 171);
            this.txtWorkplanType.Name = "txtWorkplanType";
            this.txtWorkplanType.ReadOnly = true;
            this.txtWorkplanType.Size = new System.Drawing.Size(225, 20);
            this.txtWorkplanType.TabIndex = 19;
            // 
            // lblWorkplanType
            // 
            this.lblWorkplanType.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblWorkplanType.AutoSize = true;
            this.lblWorkplanType.Location = new System.Drawing.Point(364, 173);
            this.lblWorkplanType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWorkplanType.Name = "lblWorkplanType";
            this.lblWorkplanType.Size = new System.Drawing.Size(83, 13);
            this.lblWorkplanType.TabIndex = 100;
            this.lblWorkplanType.Text = "Workplan Type:";
            // 
            // txtAdditionalInstructions
            // 
            this.tableLayoutPanel4.SetColumnSpan(this.txtAdditionalInstructions, 2);
            this.txtAdditionalInstructions.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.AdditionalInstructions", true));
            this.txtAdditionalInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAdditionalInstructions.Location = new System.Drawing.Point(3, 23);
            this.txtAdditionalInstructions.Multiline = true;
            this.txtAdditionalInstructions.Name = "txtAdditionalInstructions";
            this.txtAdditionalInstructions.ReadOnly = true;
            this.txtAdditionalInstructions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAdditionalInstructions.Size = new System.Drawing.Size(1068, 30);
            this.txtAdditionalInstructions.TabIndex = 22;
            // 
            // btnViewSQL
            // 
            this.btnViewSQL.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnViewSQL.Location = new System.Drawing.Point(275, 6);
            this.btnViewSQL.Margin = new System.Windows.Forms.Padding(2);
            this.btnViewSQL.Name = "btnViewSQL";
            this.btnViewSQL.Size = new System.Drawing.Size(80, 24);
            this.btnViewSQL.TabIndex = 30;
            this.btnViewSQL.Text = "View SQL";
            this.btnViewSQL.UseVisualStyleBackColor = true;
            this.btnViewSQL.Click += new System.EventHandler(this.btnViewSQL_Click);
            // 
            // queryMSRequestIDTextBox
            // 
            this.queryMSRequestIDTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.queryMSRequestIDTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.queryMSRequestIDTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.MSRequestID", true));
            this.queryMSRequestIDTextBox.Location = new System.Drawing.Point(845, 171);
            this.queryMSRequestIDTextBox.Name = "queryMSRequestIDTextBox";
            this.queryMSRequestIDTextBox.ReadOnly = true;
            this.queryMSRequestIDTextBox.Size = new System.Drawing.Size(226, 20);
            this.queryMSRequestIDTextBox.TabIndex = 14;
            // 
            // queryMSRequestIDLabel
            // 
            this.queryMSRequestIDLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.queryMSRequestIDLabel.AutoSize = true;
            this.queryMSRequestIDLabel.Location = new System.Drawing.Point(774, 173);
            this.queryMSRequestIDLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.queryMSRequestIDLabel.Name = "queryMSRequestIDLabel";
            this.queryMSRequestIDLabel.Size = new System.Drawing.Size(64, 13);
            this.queryMSRequestIDLabel.TabIndex = 103;
            this.queryMSRequestIDLabel.Text = "Request ID:";
            this.queryMSRequestIDLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtSourceTaskOrder
            // 
            this.txtSourceTaskOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceTaskOrder.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.SourceTaskOrder", true));
            this.txtSourceTaskOrder.Location = new System.Drawing.Point(123, 123);
            this.txtSourceTaskOrder.Name = "txtSourceTaskOrder";
            this.txtSourceTaskOrder.ReadOnly = true;
            this.txtSourceTaskOrder.Size = new System.Drawing.Size(225, 20);
            this.txtSourceTaskOrder.TabIndex = 104;
            // 
            // txtActivity
            // 
            this.txtActivity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtActivity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.Activity", true));
            this.txtActivity.Location = new System.Drawing.Point(454, 147);
            this.txtActivity.Name = "txtActivity";
            this.txtActivity.ReadOnly = true;
            this.txtActivity.Size = new System.Drawing.Size(225, 20);
            this.txtActivity.TabIndex = 105;
            // 
            // txtSourceActivityProject
            // 
            this.txtSourceActivityProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceActivityProject.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.SourceActivityProject", true));
            this.txtSourceActivityProject.Location = new System.Drawing.Point(845, 123);
            this.txtSourceActivityProject.Name = "txtSourceActivityProject";
            this.txtSourceActivityProject.ReadOnly = true;
            this.txtSourceActivityProject.Size = new System.Drawing.Size(226, 20);
            this.txtSourceActivityProject.TabIndex = 106;
            // 
            // lblSourceTaskOrder
            // 
            this.lblSourceTaskOrder.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSourceTaskOrder.AutoSize = true;
            this.lblSourceTaskOrder.Location = new System.Drawing.Point(16, 125);
            this.lblSourceTaskOrder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSourceTaskOrder.Name = "lblSourceTaskOrder";
            this.lblSourceTaskOrder.Size = new System.Drawing.Size(100, 13);
            this.lblSourceTaskOrder.TabIndex = 107;
            this.lblSourceTaskOrder.Text = "Source Task Order:";
            // 
            // lblSourceActivity
            // 
            this.lblSourceActivity.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSourceActivity.AutoSize = true;
            this.lblSourceActivity.Location = new System.Drawing.Point(366, 125);
            this.lblSourceActivity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSourceActivity.Name = "lblSourceActivity";
            this.lblSourceActivity.Size = new System.Drawing.Size(81, 13);
            this.lblSourceActivity.TabIndex = 108;
            this.lblSourceActivity.Text = "Source Activity:";
            // 
            // lblSourceActivityProject
            // 
            this.lblSourceActivityProject.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSourceActivityProject.AutoSize = true;
            this.lblSourceActivityProject.Location = new System.Drawing.Point(721, 125);
            this.lblSourceActivityProject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSourceActivityProject.Name = "lblSourceActivityProject";
            this.lblSourceActivityProject.Size = new System.Drawing.Size(117, 13);
            this.lblSourceActivityProject.TabIndex = 109;
            this.lblSourceActivityProject.Text = "Source Activity Project:";
            // 
            // descriptionBrowser
            // 
            this.descriptionBrowser.AllowNavigation = false;
            this.descriptionBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionBrowser.IsWebBrowserContextMenuEnabled = false;
            this.descriptionBrowser.Location = new System.Drawing.Point(0, 0);
            this.descriptionBrowser.Margin = new System.Windows.Forms.Padding(4);
            this.descriptionBrowser.Name = "descriptionBrowser";
            this.descriptionBrowser.Size = new System.Drawing.Size(1064, 86);
            this.descriptionBrowser.TabIndex = 115;
            this.descriptionBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // txtReportAggregationLevel
            // 
            this.txtReportAggregationLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReportAggregationLevel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.ReportAggregationLevel", true));
            this.txtReportAggregationLevel.Location = new System.Drawing.Point(845, 99);
            this.txtReportAggregationLevel.Name = "txtReportAggregationLevel";
            this.txtReportAggregationLevel.ReadOnly = true;
            this.txtReportAggregationLevel.Size = new System.Drawing.Size(226, 20);
            this.txtReportAggregationLevel.TabIndex = 111;
            // 
            // lblReportAggregationLevel
            // 
            this.lblReportAggregationLevel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblReportAggregationLevel.AutoSize = true;
            this.lblReportAggregationLevel.Location = new System.Drawing.Point(695, 101);
            this.lblReportAggregationLevel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReportAggregationLevel.Name = "lblReportAggregationLevel";
            this.lblReportAggregationLevel.Size = new System.Drawing.Size(143, 13);
            this.lblReportAggregationLevel.TabIndex = 112;
            this.lblReportAggregationLevel.Text = "Level of Report Aggregation:";
            // 
            // browserPanel
            // 
            this.browserPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel5.SetColumnSpan(this.browserPanel, 2);
            this.browserPanel.Controls.Add(this.descriptionBrowser);
            this.browserPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browserPanel.Location = new System.Drawing.Point(4, 24);
            this.browserPanel.Margin = new System.Windows.Forms.Padding(4);
            this.browserPanel.Name = "browserPanel";
            this.browserPanel.Size = new System.Drawing.Size(1066, 88);
            this.browserPanel.TabIndex = 113;
            // 
            // buttonPanel
            // 
            this.buttonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPanel.AutoScroll = true;
            this.buttonPanel.AutoScrollMinSize = new System.Drawing.Size(1200, 0);
            this.buttonPanel.Location = new System.Drawing.Point(0, 1220);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(1023, 67);
            this.buttonPanel.TabIndex = 116;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoScrollMinSize = new System.Drawing.Size(0, 900);
            this.panel1.Controls.Add(this.splitContainer3);
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1091, 707);
            this.panel1.TabIndex = 117;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 218);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer3.Size = new System.Drawing.Size(1074, 682);
            this.splitContainer3.SplitterDistance = 176;
            this.splitContainer3.TabIndex = 117;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel4);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel5);
            this.splitContainer1.Size = new System.Drawing.Size(1074, 176);
            this.splitContainer1.SplitterDistance = 56;
            this.splitContainer1.TabIndex = 115;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(lblAdditionalInstructions, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtAdditionalInstructions, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1074, 56);
            this.tableLayoutPanel4.TabIndex = 62;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(queryDescriptionLabel, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.browserPanel, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1074, 116);
            this.tableLayoutPanel5.TabIndex = 114;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanel2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel3);
            this.splitContainer2.Size = new System.Drawing.Size(1074, 502);
            this.splitContainer2.SplitterDistance = 238;
            this.splitContainer2.TabIndex = 116;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.cbRequestFileList, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.vpRequest, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblRequest, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.chkGeneratePATIDList, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1074, 238);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // vpRequest
            // 
            this.vpRequest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel2.SetColumnSpan(this.vpRequest, 3);
            this.vpRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vpRequest.LastView = null;
            this.vpRequest.Location = new System.Drawing.Point(4, 29);
            this.vpRequest.Margin = new System.Windows.Forms.Padding(4);
            this.vpRequest.Name = "vpRequest";
            this.vpRequest.ShowView = Lpp.Dns.DataMart.Client.Controls.ViewPanel.DisplayType.PLAIN;
            this.vpRequest.Size = new System.Drawing.Size(1066, 205);
            this.vpRequest.TabIndex = 23;
            this.vpRequest.ViewText = "";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.lblResponse, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.cbResponseFileList, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.vpResponse, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1074, 260);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // vpResponse
            // 
            this.vpResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vpResponse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel3.SetColumnSpan(this.vpResponse, 2);
            this.vpResponse.LastView = null;
            this.vpResponse.Location = new System.Drawing.Point(4, 29);
            this.vpResponse.Margin = new System.Windows.Forms.Padding(4);
            this.vpResponse.Name = "vpResponse";
            this.vpResponse.ShowView = Lpp.Dns.DataMart.Client.Controls.ViewPanel.DisplayType.PLAIN;
            this.vpResponse.Size = new System.Drawing.Size(1066, 227);
            this.vpResponse.TabIndex = 25;
            this.vpResponse.ViewText = "";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.txtRequestTimeLocal, 3, 3);
            this.tableLayoutPanel1.Controls.Add(lblNetwork, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 1, 8);
            this.tableLayoutPanel1.Controls.Add(lblSubmittedTo, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.txtRequestorCenter, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtSourceTaskOrder, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblRequesterCenter, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblSourceTaskOrder, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtTaskOrder, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.queryCreatedByUserNameTextBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblTaskOrder, 0, 6);
            this.tableLayoutPanel1.Controls.Add(queryCreatedByUserNameLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtPurposeOfUse, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblPurposeOfUse, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.queryActivityPriorityTextbox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(queryActivityPriorityLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtNetwork, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.queryNameTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(activityLabel, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.activityTextbox, 3, 0);
            this.tableLayoutPanel1.Controls.Add(lblDataMart, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtDataMart, 5, 0);
            this.tableLayoutPanel1.Controls.Add(queryNameLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(label1, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtWorkplanType, 3, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblWorkplanType, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtActivity, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblActivity, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtSourceActivity, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblSourceActivity, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtPhiDisclosureLevel, 3, 4);
            this.tableLayoutPanel1.Controls.Add(requestTimeLabel, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblPhiDisclosureLevel, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.activityDueDateTextbox, 3, 2);
            this.tableLayoutPanel1.Controls.Add(activityDueDateLabel, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBox2, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtActivityProject, 5, 6);
            this.tableLayoutPanel1.Controls.Add(queryIdLabel, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.queryIdTextBox, 5, 1);
            this.tableLayoutPanel1.Controls.Add(queryStatusTypeIdLabel, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblSourceActivityProject, 4, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblActivityProject, 4, 6);
            this.tableLayoutPanel1.Controls.Add(this.queryStatusTypeIdTextBox, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.queryMSRequestIDTextBox, 5, 7);
            this.tableLayoutPanel1.Controls.Add(requestorEmailLabel, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.queryMSRequestIDLabel, 4, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtSourceActivityProject, 5, 5);
            this.tableLayoutPanel1.Controls.Add(this.requestorEmailTextBox, 5, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblReportAggregationLevel, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtReportAggregationLevel, 5, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1074, 218);
            this.tableLayoutPanel1.TabIndex = 114;
            // 
            // txtRequestTimeLocal
            // 
            this.txtRequestTimeLocal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRequestTimeLocal.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queryDataMartBindingSource, "Source.CreatedOnLocal", true, System.Windows.Forms.DataSourceUpdateMode.Never, null, "g"));
            this.txtRequestTimeLocal.Location = new System.Drawing.Point(454, 75);
            this.txtRequestTimeLocal.Name = "txtRequestTimeLocal";
            this.txtRequestTimeLocal.ReadOnly = true;
            this.txtRequestTimeLocal.Size = new System.Drawing.Size(225, 20);
            this.txtRequestTimeLocal.TabIndex = 115;
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.btnClearCache);
            this.pnlFooter.Controls.Add(this.btnAddFile);
            this.pnlFooter.Controls.Add(btn_Close);
            this.pnlFooter.Controls.Add(this.btnExportResults);
            this.pnlFooter.Controls.Add(this.btnRun);
            this.pnlFooter.Controls.Add(this.btnDeleteFile);
            this.pnlFooter.Controls.Add(this.btnUploadResults);
            this.pnlFooter.Controls.Add(this.btnHold);
            this.pnlFooter.Controls.Add(this.btnViewSQL);
            this.pnlFooter.Controls.Add(this.btnPostProcess);
            this.pnlFooter.Controls.Add(this.btnRejectQuery);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 707);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1091, 34);
            this.pnlFooter.TabIndex = 118;
            // 
            // btnClearCache
            // 
            this.btnClearCache.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClearCache.Enabled = false;
            this.btnClearCache.Location = new System.Drawing.Point(661, 6);
            this.btnClearCache.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearCache.Name = "btnClearCache";
            this.btnClearCache.Size = new System.Drawing.Size(110, 24);
            this.btnClearCache.TabIndex = 37;
            this.btnClearCache.Text = "Clear Cache";
            this.btnClearCache.UseVisualStyleBackColor = true;
            this.btnClearCache.Click += new System.EventHandler(this.btnClearCache_Click);
            // 
            // chkGeneratePATIDList
            // 
            this.chkGeneratePATIDList.AutoSize = true;
            this.chkGeneratePATIDList.Location = new System.Drawing.Point(63, 3);
            this.chkGeneratePATIDList.Name = "chkGeneratePATIDList";
            this.chkGeneratePATIDList.Size = new System.Drawing.Size(124, 17);
            this.chkGeneratePATIDList.TabIndex = 83;
            this.chkGeneratePATIDList.Text = "Generate PATID List";
            this.chkGeneratePATIDList.UseVisualStyleBackColor = true;
            this.chkGeneratePATIDList.CheckedChanged += new System.EventHandler(this.chkGeneratePATIDList_CheckedChanged);
            // 
            // RequestDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CancelButton = btn_Close;
            this.ClientSize = new System.Drawing.Size(1091, 741);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(lblSubmittedByNote);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlFooter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(1020, 398);
            this.Name = "RequestDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DataMart Client - Request Detail";
            this.Load += new System.EventHandler(this.RequestDetailForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.queryDataMartBindingSource)).EndInit();
            this.browserPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource queryDataMartBindingSource;
        private System.Windows.Forms.TextBox queryIdTextBox;
        private System.Windows.Forms.TextBox queryCreatedByUserNameTextBox;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnUploadResults;
        private System.Windows.Forms.Button btnRejectQuery;
        private System.Windows.Forms.TextBox queryStatusTypeIdTextBox;
        private System.Windows.Forms.TextBox queryNameTextBox;
        private System.Windows.Forms.TextBox requestorEmailTextBox;
        private System.Windows.Forms.Button btnHold;
        private System.Windows.Forms.TextBox txtNetwork;
        private System.Windows.Forms.TextBox txtDataMart;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnExportResults;
        private System.Windows.Forms.TextBox activityDueDateTextbox;
        private System.Windows.Forms.TextBox activityTextbox;
        private System.Windows.Forms.TextBox queryActivityPriorityTextbox;
        private Controls.DataMartViewPanel vpRequest;
        public Controls.DataMartViewPanel vpResponse;
        private System.Windows.Forms.Button btnDeleteFile;
        private System.Windows.Forms.Button btnAddFile;
        private System.Windows.Forms.OpenFileDialog FileDialog;
        private System.ComponentModel.BackgroundWorker processRequestWorker;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label lblRequest;
        private System.Windows.Forms.Label lblResponse;
        private System.Windows.Forms.CheckBox cbRequestFileList;
        private System.Windows.Forms.CheckBox cbResponseFileList;
        private System.Windows.Forms.Button btnPostProcess;
        private System.Windows.Forms.TextBox txtPurposeOfUse;
        private System.Windows.Forms.Label lblPurposeOfUse;
        private System.Windows.Forms.Label lblPhiDisclosureLevel;
        private System.Windows.Forms.Label lblTaskOrder;
        private System.Windows.Forms.Label lblActivity;
        private System.Windows.Forms.Label lblActivityProject;
        private System.Windows.Forms.TextBox txtPhiDisclosureLevel;
        private System.Windows.Forms.TextBox txtTaskOrder;
        private System.Windows.Forms.TextBox txtActivityProject;
        private System.Windows.Forms.TextBox txtSourceActivity;
        private System.Windows.Forms.Label lblRequesterCenter;
        private System.Windows.Forms.TextBox txtRequestorCenter;
        private System.Windows.Forms.TextBox txtWorkplanType;
        private System.Windows.Forms.Label lblWorkplanType;
        private System.Windows.Forms.TextBox txtAdditionalInstructions;
        private System.Windows.Forms.Button btnViewSQL;
        private System.Windows.Forms.TextBox queryMSRequestIDTextBox;
        private System.Windows.Forms.Label queryMSRequestIDLabel;
        private System.Windows.Forms.TextBox txtSourceTaskOrder;
        private System.Windows.Forms.TextBox txtActivity;
        private System.Windows.Forms.TextBox txtSourceActivityProject;
        private System.Windows.Forms.Label lblSourceTaskOrder;
        private System.Windows.Forms.Label lblSourceActivity;
        private System.Windows.Forms.Label lblSourceActivityProject;
        private System.Windows.Forms.WebBrowser descriptionBrowser;
        private System.Windows.Forms.TextBox txtReportAggregationLevel;
        private System.Windows.Forms.Label lblReportAggregationLevel;
        private System.Windows.Forms.Panel browserPanel;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtRequestTimeLocal;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnClearCache;
        private System.Windows.Forms.CheckBox chkGeneratePATIDList;
    }
}