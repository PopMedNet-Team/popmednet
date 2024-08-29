using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Lpp.Dns.DataMart.Client.Controls
{
    public partial class JSON : UserControl
    {
        public JSON()
        {
            InitializeComponent();
            HasResults = false;
        }

        public bool HasResults { get; internal set; }

        internal void InitializeDataSourceStream(Stream data)
        {
            //try to deserialize to QueryComposer response, on fail use generic grid view
            try
            {
                var serializationSettings = new Newtonsoft.Json.JsonSerializerSettings();
                serializationSettings.Converters.Add(new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionConverter());
                var deserializer = Newtonsoft.Json.JsonSerializer.Create(serializationSettings);
                var loadSettings = new Newtonsoft.Json.Linq.JsonLoadSettings { CommentHandling = Newtonsoft.Json.Linq.CommentHandling.Ignore, LineInfoHandling = Newtonsoft.Json.Linq.LineInfoHandling.Ignore };

                DTO.QueryComposer.QueryComposerResponseDTO responseDTO = null;

                var jObj = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.Linq.JObject.ReadFrom(new Newtonsoft.Json.JsonTextReader(new StreamReader(data)), loadSettings);

                if (jObj.ContainsKey("SchemaVersion"))
                {
                    responseDTO = jObj.ToObject<DTO.QueryComposer.QueryComposerResponseDTO>(deserializer);
                }
                else
                {
                    //assume it is pre multi-query response
                    var queryDTO = jObj.ToObject<DTO.QueryComposer.QueryComposerResponseQueryResultDTO>(deserializer);

                    responseDTO = new DTO.QueryComposer.QueryComposerResponseDTO
                    {
                        Queries = new[] { queryDTO }
                    };
                }

                HasResults = responseDTO.Queries.SelectMany(q => { return q.Results == null ? new int[] { 0 } : q.Results.Select(r => r.Count()); }).Sum() > 0;

                this.SuspendLayout();
                this.Controls.Clear();

                int yPosition = 0;
                int index = 0;
                foreach(var queryResult in responseDTO.Queries)
                {
                    
                    var titleLabel = new Label();
                    titleLabel.AutoSize = true;
                    titleLabel.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    titleLabel.TextAlign = ContentAlignment.MiddleLeft;
                    titleLabel.AutoSize = false;
                    titleLabel.Size = new Size(this.Width, 23);
                    titleLabel.Location = new Point(0, 0);
                    titleLabel.Padding = new Padding(3);
                    titleLabel.Name = $"lblTitle_{index}";
                    titleLabel.Text = $"Cohort {index + 1}: {queryResult.Name}";
                    titleLabel.ForeColor = SystemColors.InactiveCaptionText;

                    var resultGrid = CreateDataGridView("grid" + index, FromQueryComposerQueryResult(queryResult));
                    resultGrid.Location = new Point(0, titleLabel.Height);
                    resultGrid.Dock = DockStyle.None;
                    resultGrid.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                    var totalNumberOfResultRows = (from rs in queryResult.Results
                                                   select rs.Count()).Sum();

                    var gridHeight = resultGrid.ColumnHeadersHeight + (totalNumberOfResultRows * resultGrid.RowTemplate.Height);

                    if(gridHeight < 150)
                    {
                        gridHeight = 150;
                    }
                    resultGrid.Size = new Size(this.Width, gridHeight);

                    var layoutPanel = new Panel();
                    layoutPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                    layoutPanel.Name = "layoutPanel" + index;
                    layoutPanel.Size = new Size(this.Width, resultGrid.Bottom);
                    layoutPanel.TabIndex = index;
                    layoutPanel.BackColor = SystemColors.GradientInactiveCaption;
                    layoutPanel.Location = new Point(0, yPosition);
                    layoutPanel.Controls.AddRange(new Control[]{ resultGrid, titleLabel });
                    yPosition += layoutPanel.ClientRectangle.Height;

                    this.Controls.Add(layoutPanel);

                    index++;
                }


                if(this.Controls.Count == 1)
                {
                    this.Controls[0].Size = new Size(this.Controls[0].Size.Width, this.Size.Height);
                    this.Controls[0].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

                    ((Panel)this.Controls[0]).PerformLayout();
                }
                else
                {
                    this.Size = new Size(this.Width, yPosition);
                }

                this.ResumeLayout();

            }
            catch(Exception ex)
            {
                using(var reader = new StreamReader(data))
                {
                    var grid = CreateDataGridView("response1", TransformJSONToDataTable(reader.ReadToEnd()));

                    this.Controls.Clear();
                    this.Controls.Add(grid);
                }
            }
        }

        DataTable FromQueryComposerQueryResult(DTO.QueryComposer.QueryComposerResponseQueryResultDTO queryResult)
        {
            var table = new DataTable();
            foreach(var property in queryResult.Properties)
            {
                var column = new DataColumn();
                column.ColumnName = property.As;
                column.DataType = Nullable.GetUnderlyingType(property.AsType()) ?? property.AsType();
                column.Caption = property.As;
                table.Columns.Add(column);
            }

            foreach(var row in queryResult.Results.First())
            {
                var datarow = table.NewRow();
                foreach(DataColumn column in table.Columns)
                {
                    var obj = row[column.ColumnName];
                    datarow[column.ColumnName] = obj == null ? DBNull.Value : obj;
                }
                table.Rows.Add(datarow);
            }

            return table;
        }


        private DataTable TransformJSONToDataTable(string json)
        {
            var results = new DataTable();
            var response = JObject.Parse(json);
            foreach (var row in response["Results"][0])
            {
                var datarow = results.NewRow();
                foreach (var jToken in row)
                {
                    var jproperty = jToken as JProperty;
                    if (jproperty == null)
                        continue;

                    if (results.Columns[jproperty.Name] == null)
                        results.Columns.Add(jproperty.Name, typeof(object));

                    datarow[jproperty.Name] = jproperty.Value.ToObject(typeof(object));
                }
                if (!results.Columns.Contains("LowThreshold"))
                {
                    results.Columns.Add("LowThreshold", typeof(string));
                    datarow["LowThreshold"] = "False";
                }
                if (datarow["LowThreshold"].ToString() == "")
                {
                    datarow["LowThreshold"] = "False";
                }

                results.Rows.Add(datarow);
            }

            return results;
        }

        DataGridView CreateDataGridView(string name, DataTable datasource)
        {
            var grid = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();

            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToOrderColumns = false;
            grid.AllowUserToDeleteRows = false;

            grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            grid.Margin = new Padding(0, 0, 5, 20);

            grid.Name = name;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowTemplate.Height = 24;
            grid.TabIndex = 1;
            grid.Visible = true;
            grid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.JSON_CellFormatting);

            if (datasource != null)
            {
                grid.AutoGenerateColumns = true;
                grid.DataSource = datasource;
            }

            ((System.ComponentModel.ISupportInitialize)grid).EndInit();

            return grid;
        }

        void JSON_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var grid = (DataGridView)sender;

            if (e.Value == null || e.Value == DBNull.Value)
            {
                e.CellStyle.BackColor = Color.LightGray;
                e.Value = "<< NULL >>";
            }

            if (grid.Columns.Contains("LowThreshold"))
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(grid.Rows[e.RowIndex].Cells["LowThreshold"].Value.ToString(), "true"))
                {
                    e.CellStyle.BackColor = Color.Yellow;
                }

                grid.Columns["LowThreshold"].Visible = false;
            }

            
        }

    }
}
