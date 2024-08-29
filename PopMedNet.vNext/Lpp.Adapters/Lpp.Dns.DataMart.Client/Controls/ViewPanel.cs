﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using Lpp.Dns.DataMart.Model;
using Lpp.Dns.DataMart.Lib.Css;
using Newtonsoft.Json.Linq;

namespace Lpp.Dns.DataMart.Client.Controls
{
    public partial class ViewPanel : UserControl
    {
        /// <summary>
        /// Represents real controls on the UI.
        /// </summary>
        public enum DisplayType
        {
            PLAIN,
            JSON,
            HTML,
            DATASET,
            FILELIST,
            XSLXML,
            XML,
            URL
        }

        IList<Control> views = new List<Control>();
        DisplayType _displayType = DisplayType.PLAIN;
        Control _currentView = null;

        public Control LastView
        {
            get;
            set;
        }

        public Control View
        {
            get
            {
                return _currentView;
            }

            set
            {
                _currentView = value;
                foreach (Control view in views)
                    view.Visible = view == _currentView;
            }
        }

        public DisplayType ShowView
        {
            get
            {
                return _displayType;
            }
            set
            {
                _displayType = value;
                foreach (Control view in views)
                {
                    if(view.Name.Equals(value.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        View = view;
                        break;
                    }
                }
            }
        }

        public string ShowMimeType
        {
            set
            {
                string[] mimeTypeParts = value.Split('/');
                string viewType = "PLAIN";
                if (mimeTypeParts.Length > 1)
                    viewType = mimeTypeParts[1].ToUpper();

                switch (viewType)
                {
                    case "JSON":
                        viewType = "JSON";
                        _displayType = DisplayType.JSON;
                        break;
                    case "LPP-DNS-TABLE":
                        viewType = "DATASET";
                        _displayType = DisplayType.DATASET;
                        break;
                    case "LPP-DNS-UIARGS":
                        viewType = "XSLXML";
                        _displayType = DisplayType.XSLXML;
                        break;
                    case "HTML":
                        viewType = "HTML";
                        _displayType = DisplayType.HTML;
                        break; 
                    case "URL":
                        viewType = "URL";
                        _displayType = DisplayType.URL;
                        break;
                    case "XML":
                        viewType = "XML";
                        _displayType = DisplayType.XML;
                        break;
                    default:
                        viewType = "PLAIN";
                        _displayType = DisplayType.PLAIN;
                        break;
                }

                foreach (Control view in views)
                {
                    if(view.Name.Equals(viewType, StringComparison.OrdinalIgnoreCase))
                    {
                        View = view;
                        break;
                    }
                }
            }
        }

        public IEnumerable<Document> FileListDataSource
        {
            set
            {
                bsDocumentList.DataSource = value;
            }
        }

        public bool HasDocuments
        {
            get
            {
                return bsDocumentList.Count > 0;
            }
        }

        public object DataSource
        {
            set
            {
                HtmlDocument html = null;
                switch (View.Name)
                {
                    case "PLAIN":
                        ((RichTextBox) View).Text = (string) value;
                        lblNoResults.Visible = false;
                        break;
                    case "HTML":
                        html = ((WebBrowser)View).Document.OpenNew(true);
                        html.Write((string)value);
                        lblNoResults.Visible = false;
                        break;
                    case "URL":
                        URL.Navigate(value.ToString());
                        lblNoResults.Visible = false;
                        break;
                    case "DATASET":
                        DataTable data = (DataTable)value;
                        ((DataGridView)View).DataSource = data;
                        lblNoResults.Visible = data == null || data.Rows.Count == 0;
                        break;
                    case "XSLXML":
                        html = ((WebBrowser)View).Document.OpenNew(true);
                        html.Write(TransformToHTML((string)value));
                        lblNoResults.Visible = false;                   
                        break;
                    case "XML":
                        TransformToTreeView((string) value, (TreeView)View);
                        break;
                    case "JSON":

                        this.JSON.InitializeDataSourceStream(_datasourceStream);
                        this.JSON.Dock = DockStyle.Fill;
                        this.JSON.AutoScroll = true;
                        lblNoResults.Visible = this.JSON.HasResults == false;

                        break;     
                    default:
                        lblNoResults.Visible = false;
                        break;
                }
                
            }
        }

        #region XML Tree View

        private void TransformToTreeView(string xmlString, TreeView treeXml)
        {
            treeXml.Nodes.Clear();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            ConvertXmlNodeToTreeNode(doc, treeXml.Nodes);

            lblNoResults.Visible = treeXml.Nodes.Count == 0;
        }

        private void Expand(TreeNode treeNode)
        {
            treeNode.Expand();
            foreach (TreeNode child in treeNode.Nodes)
                Expand(child);
        }

        private void Expand(object sender, EventArgs ev)
        {
            TreeNode treeNode = (TreeNode) ((MenuItem) sender).Tag;
            Expand(treeNode);
        }

        private void Collapse(object sender, EventArgs ev)
        {
            ((TreeNode) ((MenuItem) sender).Tag).Collapse();
        }

        private void ConvertXmlNodeToTreeNode(XmlNode xmlNode, TreeNodeCollection treeNodes)
        {

            TreeNode newTreeNode = treeNodes.Add(xmlNode.Name);
            newTreeNode.ContextMenu = new ContextMenu();
            MenuItem expandMenu = new MenuItem("Expand");
            expandMenu.Tag = newTreeNode;
            expandMenu.Click += new EventHandler(Expand);
            newTreeNode.ContextMenu.MenuItems.Add(expandMenu);
            MenuItem collapseMenu = new MenuItem("Collapse");
            collapseMenu.Tag = newTreeNode;
            collapseMenu.Click += new EventHandler(Collapse);
            newTreeNode.ContextMenu.MenuItems.Add(collapseMenu);

            switch (xmlNode.NodeType)
            {
                case XmlNodeType.ProcessingInstruction:
                case XmlNodeType.XmlDeclaration:
                    newTreeNode.Text = "<?" + xmlNode.Name + " " +
                      xmlNode.Value + "?>";
                    break;
                case XmlNodeType.Element:
                    newTreeNode.Text = "<" + xmlNode.Name + ">";
                    break;
                case XmlNodeType.Attribute:
                    newTreeNode.Text = "ATTRIBUTE: " + xmlNode.Name;
                    break;
                case XmlNodeType.Text:
                case XmlNodeType.CDATA:
                    newTreeNode.Text = xmlNode.Value;
                    break;
                case XmlNodeType.Comment:
                    newTreeNode.Text = "<!--" + xmlNode.Value + "-->";
                    break;
            }

            if (xmlNode.Attributes != null)
            {
                foreach (XmlAttribute attribute in xmlNode.Attributes)
                {
                    ConvertXmlNodeToTreeNode(attribute, newTreeNode.Nodes);
                }
            }
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                ConvertXmlNodeToTreeNode(childNode, newTreeNode.Nodes);
            }

            
        }

        #endregion // XML TreeView


        // BMS: Move this outside the views, let the caller figure out how to build the values that populate the views

        private string TransformToHTML(string xslxml)
        {
            int startIndex = xslxml.IndexOf("<xsl:stylesheet");
            int length = xslxml.IndexOf("</xsl:stylesheet>") + "</xsl:stylesheet>".Length - startIndex;
            string xsl = xslxml.Substring(startIndex, length);

            byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(xslxml);
            XPathDocument xml = new XPathDocument(new MemoryStream(bytes));
            XslCompiledTransform xslt = new XslCompiledTransform();
            using (XmlTextReader transform = new XmlTextReader(new MemoryStream(System.Text.UTF32Encoding.UTF8.GetBytes(xsl))))
            {
                using (var writer = new StringWriter())
                {
                    try
                    {
                        xslt.Load(transform);
                        xslt.Transform(xml, null, writer);
                    }
                    catch
                    {
                    }

                    string html = writer.ToString();
                    //log.Debug(html);
                    return html;
                }
            }

        }

        System.IO.MemoryStream _datasourceStream = null;

        public void SetDataSourceStream(Stream value, ViewableDocumentStyle style = null)
        {
            if(_datasourceStream != null)
            {
                _datasourceStream.Dispose();
                _datasourceStream = null;
            }

            switch (View.Name)
            {
                case "PLAIN":
                case "HTML":
                case "XSLXML":
                case "XML":
                    string content = new StreamReader( value ).ReadToEnd();
                    DataSource = content;
                    break;

                case "JSON":

                    _datasourceStream = new MemoryStream();
                    value.CopyTo(_datasourceStream);
                    value.Flush();
                    _datasourceStream.Position = 0;

                    DataSource = _datasourceStream;

                    break;
                    
                case "DATASET":
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXml(value, XmlReadMode.Auto);
                    //PMN-623-DMC: Results View Scrolling laterally. Set the column-width to fit the content of all cells including header cells.
                    ((DataGridView)View).AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    ((DataGridView)View).DataSource = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : null;

                    lblNoResults.Visible = dataSet.Tables.Count <= 0 || dataSet.Tables[0].Rows.Count <= 0;
                    lblNoResults.BringToFront();

                    // Changing row color
                    if (style != null)
                    {
                        CSSParser parser = new CSSParser();
                        parser.Read(style.Css);

                        foreach (int rn in style.StyledRows)
                        {
                            DataGridViewRow row = ((DataGridView)View).Rows[rn];
                            ApplyStyle(row, parser.Classes["StyledRows"]);
                        }
                    }
                    break;
               
                    
                default:
                    break;
            }
        }

        /// <summary>
        /// Gets or sets the text content of the Plain text visual control.
        /// </summary>
        public string ViewText
        {
            get
            {
                return PLAIN.Text;
            }

            set
            {
                PLAIN.Text = value;
            }
        }

        public ViewPanel()
        {
            InitializeComponent();

            //You _must_ initialize the browser before trying to set the document text, else the first time you try to set the content it will get swallowed by the control.
            HTML.Navigate("about:blank");
            URL.Navigate("about:blank");
            XSLXML.Navigate("about:blank");
            lblNoResults.Visible = false;
            Array displayTypes = (Array) Enum.GetValues(typeof(DisplayType)).Cast<DisplayType>();
            foreach (DisplayType displayType in displayTypes)
            {
                foreach (Control control in Controls)
                {
                    if (control.Name == displayType.ToString())
                        views.Add(control);
                }
            }
        }

        public void ApplyStyle(DataGridViewRow row, Dictionary<string, string> style)
        {
            DataGridViewCellStyle cellStyle = row.DefaultCellStyle;

            foreach(string key in style.Keys)
            {
                switch(key)
                {
                    case "background-color":
                        cellStyle.BackColor = ColorTranslator.FromHtml(style["background-color"]);
                        break;
                    case "color":
                        cellStyle.ForeColor = ColorTranslator.FromHtml(style["color"]);
                        break;
                    default:
                        // TODO padding, font, etc.
                        break;
                }
            }
        }

        public void ToggleFileView(object sender, EventArgs e)
        {
            if (View != FILELIST)
            {
                LastView = View;
                ShowView = DisplayType.FILELIST;
                FILELIST.BringToFront();
            }
            else
            {
                View = LastView;
            }
        }

        private void JSON_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null || e.Value == DBNull.Value)
            {
                e.CellStyle.BackColor = Color.LightGray;
                e.Value = "<< NULL >>";
            }

            if (this.JSON_OLD.Columns.Contains("LowThreshold") && StringComparer.OrdinalIgnoreCase.Equals(this.JSON_OLD.Rows[e.RowIndex].Cells["LowThreshold"].Value.ToString(), "true"))
            {
                e.CellStyle.BackColor = Color.Yellow;
            }
            this.JSON_OLD.Columns["LowThreshold"].Visible = false;


        }

        protected void RemoveSelectColumn()
        {
            FILELIST.Columns.Remove(colDocumentSelected);
        }

        protected void SetColumnsForAttachments()
        {
            colMimeType.FillWeight = 30f;
            colSize.FillWeight = 30f;
            FILELIST.Columns["colDocumentID"].Visible = false;

        }
    }

}
