using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Lpp.Dns.DataMart.Client.Properties;
using System.Text.RegularExpressions;

using Lpp.Dns.DataMart.Model;
using Lpp.Dns.DataMart.Lib;
using System.Collections;
using System.Reflection;
using Lpp.Dns.DataMart.Lib.Classes;
using System.IO;
using Lpp.Dns.DataMart.Lib.Utils;
using Lpp.Dns.DataMart.Client.Utils;
using log4net;

namespace Lpp.Dns.DataMart.Client
{
    public partial class ModelSettingsForm : Form
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IModelProcessor modelProcessor;
        HashSet<Control> _editors;

        public ModelSettingsForm()
        {
            InitializeComponent();
        }

        public DataMartDescription DataMartDescription
        {
            get;
            set;
        }

        public NetWorkSetting NetworkSetting
        {
            get;
            set;
        }

        public HubModel HubModel
        {
            get;
            set;
        }

        private ModelDescription ModelDescription
        {
            get
            {
                var lst = DataMartDescription.ModelList;
                if (lst == null)
                {
                    DataMartDescription.ModelList = lst = new List<ModelDescription>();
                }

                var res = lst.FirstOrDefault( d => d.ModelId == HubModel.Id );
                if (res == null)
                {
                    lst.Add(res = new ModelDescription { ModelId = HubModel.Id, ModelName = HubModel.Name, ProcessorId = HubModel.ModelProcessorId });
                }

                if (res.Properties == null)
                {
                    res.Properties = new List<PropertyData>();
                }

                return res;
            }
        }        

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            Lpp.Dns.DataMart.Client.DomainManger.DomainManager domainManager = new DomainManger.DomainManager(Configuration.PackagesFolderPath);
            try
            {
                if (ModelDescription == null)
                    return;

                if(ModelDescription.ProcessorId == Guid.Empty)
                    ModelDescription.ProcessorId = HubModel.ModelProcessorId;
               
                //get the package identifier and version
                var packageIdentifier = DnsServiceManager.GetRequestTypeIdentifier(NetworkSetting, ModelDescription.ModelId, ModelDescription.ProcessorId);
                if (!System.IO.File.Exists(System.IO.Path.Combine(Configuration.PackagesFolderPath, packageIdentifier.PackageName())))
                {
                    DnsServiceManager.DownloadPackage(NetworkSetting, packageIdentifier);
                }
                domainManager.Load(packageIdentifier.Identifier, packageIdentifier.Version);
                modelProcessor = domainManager.GetProcessor(ModelDescription.ProcessorId);
                ProcessorManager.UpdateProcessorSettings(ModelDescription, modelProcessor);
                
                if (modelProcessor is IEarlyInitializeModelProcessor)
                {
                    ((IEarlyInitializeModelProcessor)modelProcessor).Initialize(ModelDescription.ModelId, Array.Empty<DocumentWithStream>());
                }

                this.SuspendLayout();
                
                if (modelProcessor.ModelMetadata.Settings == null || !modelProcessor.ModelMetadata.Settings.Any())
                {
                    var noSettingsLabel = new Label();
                    noSettingsLabel.Text = "This model processor does not have any settings.";
                    noSettingsLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;
                    noSettingsLabel.TextAlign = ContentAlignment.MiddleCenter;
                    tableLayoutPanel1.Controls.Add(noSettingsLabel, 0, 0);
                    tableLayoutPanel1.SetColumnSpan(noSettingsLabel, 2);
                }
                else
                {
                    _editors = new HashSet<Control>();
                    tableLayoutPanel1.RowStyles.Clear();
                    int rowIndex = 0;

                    IEnumerable<Lpp.Dns.DataMart.Model.Settings.ProcessorSetting> settings = modelProcessor.ModelMetadata.Settings;
                    if (modelProcessor.ModelMetadata.SQlProviders != null && modelProcessor.ModelMetadata.SQlProviders.Any())
                    {                        
                        DataMart.Client.Controls.DataSourceEditor dsEditor = new Controls.DataSourceEditor(modelProcessor.ModelMetadata, ModelDescription.Properties);
                        dsEditor.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;

                        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                        tableLayoutPanel1.Controls.Add(dsEditor, 0, rowIndex++);
                        tableLayoutPanel1.SetColumnSpan(dsEditor, 2);

                        settings = settings.Where(s => !Lpp.Dns.DataMart.Model.Settings.ProcessorSettings.IsDbSetting(s.Key)).ToArray();
                        _editors.Add(dsEditor);
                    }

                    settings.ToList().ForEach(s => {

                        string value = ModelDescription.Properties.Where(p => string.Equals(p.Name, s.Key, StringComparison.OrdinalIgnoreCase)).Select(p => p.Value).FirstOrDefault();

                        if (string.IsNullOrEmpty(value) && s.Required && !string.IsNullOrEmpty(s.DefaultValue))
                        {
                            value = s.DefaultValue;
                        }

                        Label lbl = new Label();
                        lbl.AutoSize = true;
                        lbl.Anchor = AnchorStyles.Right;
                        lbl.TextAlign = ContentAlignment.MiddleRight;
                        lbl.Text = s.Title.EndsWith(":") ? s.Title : s.Title + ":";

                        Control editor = null;
                        if (s.ValidValues != null)
                        {
                            ComboBox combo = new ComboBox();
                            combo.DropDownStyle = ComboBoxStyle.DropDownList;
                            combo.Anchor = AnchorStyles.Right | AnchorStyles.Left;
                            combo.Name = s.Key;

                            combo.Items.AddRange(s.ValidValues.Select(v => new PropertyData(v.Key, v.Value.ToString())).ToArray());
                            if (!string.IsNullOrEmpty(value))
                            {
                                foreach (PropertyData p in combo.Items)
                                {
                                    if (string.Equals(p.Value, value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        combo.SelectedItem = p;
                                        break;
                                    }
                                }
                            }

                            if (combo.SelectedIndex < 0)
                                combo.SelectedIndex = 0;

                            editor = combo;
                        }
                        else
                        {
                            if (s.ValueType == typeof(bool) || s.ValueType == typeof(Nullable<bool>))
                            {                                
                                CheckBox chkbox = new CheckBox();
                                chkbox.Anchor = AnchorStyles.Left;
                                chkbox.Text = s.Title;
                                chkbox.TextAlign = ContentAlignment.MiddleLeft;
                                chkbox.AutoSize = true;

                                if (!string.IsNullOrEmpty(value))
                                {
                                    bool isChecked = false;
                                    bool.TryParse(value, out isChecked);
                                    chkbox.Checked = isChecked;
                                }

                                editor = chkbox;
                                lbl = null;
                            }
                            else if (s.ValueType == typeof(Lpp.Dns.DataMart.Model.Settings.FilePickerEditor))
                            {
                                SelectFileButton btn = new SelectFileButton(s.EditorSettings as Lpp.Dns.DataMart.Model.Settings.FilePickerEditor);
                                if (btn.Multiselect)
                                {
                                    btn.FileNames = ((value ?? "").Trim(',')).Split(',');
                                }
                                else
                                {
                                    btn.FileName = value;
                                }
                                btn.Anchor = AnchorStyles.Right | AnchorStyles.Left;
                                editor = btn;
                            }
                            else if (s.ValueType == typeof(Lpp.Dns.DataMart.Model.Settings.FolderSelectorEditor))
                            {
                                SelectFolderButton btn = new SelectFolderButton(s.EditorSettings as Lpp.Dns.DataMart.Model.Settings.FolderSelectorEditor);
                                btn.FolderPath = value;
                                btn.Anchor = AnchorStyles.Right | AnchorStyles.Left;
                                editor = btn;
                            }
                            else
                            {
                                TextBox txtbox = new TextBox();
                                txtbox.Anchor = AnchorStyles.Right | AnchorStyles.Left;
                                txtbox.Text = value;
                                editor = txtbox;
                            }
                        }

                        if (editor != null)
                        {
                            editor.Tag = s.Key;
                            _editors.Add(editor);

                            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                            if (lbl != null)
                            {
                                tableLayoutPanel1.Controls.Add(lbl, 0, rowIndex);
                                tableLayoutPanel1.Controls.Add(editor, 1, rowIndex++);
                            }
                            else
                            {
                                tableLayoutPanel1.Controls.Add(editor, 0, rowIndex++);
                                tableLayoutPanel1.SetColumnSpan(editor, 2);
                            }
                        }
                    });

                    //add auto expanding row to bottom to fill empty space
                    Label emptyLabel = new Label();
                    emptyLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;
                    emptyLabel.Text = string.Empty;
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
                    tableLayoutPanel1.Controls.Add(emptyLabel, 0, rowIndex);
                    tableLayoutPanel1.SetColumnSpan(emptyLabel, 2);


                }
                this.ResumeLayout(true);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Close();
            }
            finally
            {
                domainManager.Dispose();
                this.Cursor = Cursors.Default;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ModelDescription.Properties != null)
                    ModelDescription.Properties.Clear();

                if (_editors != null)
                {
                    _editors.ForEach(ctrl =>
                    {

                        if (ctrl is DataMart.Client.Controls.DataSourceEditor)
                        {
                            ModelDescription.Properties.AddRange(((DataMart.Client.Controls.DataSourceEditor)ctrl).GetSettings());
                        }
                        else
                        {

                            string value = string.Empty;
                            if (ctrl is TextBox)
                            {
                                value = ((TextBox)ctrl).Text.Trim();
                            }

                            if (ctrl is ComboBox)
                            {
                                ComboBox combo = (ComboBox)ctrl;
                                if (combo.SelectedIndex >= 0)
                                {
                                    value = ((PropertyData)combo.SelectedItem).Value;
                                }
                            }

                            if (ctrl is CheckBox)
                            {
                                value = ((CheckBox)ctrl).Checked.ToString();
                            }

                            if (ctrl is SelectFileButton)
                            {
                                SelectFileButton btn =  (SelectFileButton)ctrl;
                                if(btn.Multiselect)
                                {
                                    value = string.Join(",", btn.FileNames);
                                }else
                                {
                                    value = btn.FileName;
                                }
                            }

                            if (ctrl is SelectFolderButton)
                            {
                                value = ((SelectFolderButton)ctrl).FolderPath;
                            }

                            if (!string.IsNullOrEmpty(ctrl.Tag.ToString()))
                                ModelDescription.Properties.Add(new PropertyData(ctrl.Tag.ToString(), value));

                        }

                    });

                    _editors.Clear();
                }

                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }

    internal class SelectFileButton : Button
    {
        readonly Lpp.Dns.DataMart.Model.Settings.FilePickerEditor _settings;
        string _filename = null;
        IEnumerable<string> _filenames = null;

        public SelectFileButton(Lpp.Dns.DataMart.Model.Settings.FilePickerEditor settings)
        {
            _settings = settings;
            this.Text = "...";
            this.TextAlign = ContentAlignment.MiddleLeft;
        }

        protected override void OnClick(EventArgs e)
        {
            using (OpenFileDialog dia = new OpenFileDialog())
            {
                dia.Filter = "All Files|*.*";                
                if (_settings != null)
                {
                    if (!string.IsNullOrWhiteSpace(_settings.Title) && dia.Title != _settings.Title)
                        dia.Title = _settings.Title;

                    if (!string.IsNullOrWhiteSpace(_settings.Filter) && dia.Filter != _settings.Filter)
                        dia.Filter = _settings.Filter;
                }
                dia.Multiselect = this.Multiselect;

                if (!dia.Multiselect && !string.IsNullOrWhiteSpace(_filename) && System.IO.File.Exists(_filename))
                {
                    dia.FileName = _filename;
                }

                if (dia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (dia.Multiselect)
                    {
                        FileNames = dia.FileNames;
                    }
                    else
                    {
                        FileName = dia.FileName;
                    }
                }
            }            
        }

        public string FileName 
        {
            get
            {
                return _filename;
            }
            set
            {
                _filenames = null;
                _filename = value;
                this.Text = string.IsNullOrWhiteSpace(_filename) ? "..." : _filename;
            }
        }

        public IEnumerable<string> FileNames 
        {
            get
            {
                return _filenames;
            }
            set
            {
                _filename = null;
                _filenames = value;
                this.Text = _filenames == null || !_filenames.Any() ? "..." : string.Join(", ", _filenames.ToArray());
            }
        
        }

        public bool Multiselect
        {
            get
            {
                if (_settings != null)
                {
                    return _settings.Multiselect;
                }
                return false;
            }
        }
    }

    internal class SelectFolderButton : Button
    {
        readonly Lpp.Dns.DataMart.Model.Settings.FolderSelectorEditor _settings;
        string _folderPath = null;

        public SelectFolderButton(Lpp.Dns.DataMart.Model.Settings.FolderSelectorEditor settings)
        {
            _settings = settings;
            this.Text = "...";
            this.TextAlign = ContentAlignment.MiddleLeft;
        }

        protected override void OnClick(EventArgs e)
        {
            using (FolderBrowserDialog dia = new FolderBrowserDialog())
            {
                if (_settings != null)
                {
                    if (!string.IsNullOrWhiteSpace(_settings.Description))
                    {
                        dia.Description = _settings.Description;
                    }
                    dia.ShowNewFolderButton = _settings.ShowNewFolderButton;
                }

                if (!string.IsNullOrWhiteSpace(_folderPath) && System.IO.Directory.Exists(_folderPath))
                    dia.SelectedPath = _folderPath;

                if (dia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FolderPath = dia.SelectedPath;
                }
            }
        }

        public string FolderPath 
        {
            get
            {
                return _folderPath;
            }
            set
            {
                _folderPath = value;
                this.Text = string.IsNullOrWhiteSpace(_folderPath) ? "..." : _folderPath;
            }
        }
    }
}