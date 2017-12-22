using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Lpp.Utilities;

namespace Lpp.Dns.DataMart.Client.Controls
{
    public partial class DataSourceEditor : UserControl
    {
        readonly Lpp.Dns.DataMart.Model.IModelMetadata ModelData;
        readonly IList<Lpp.Dns.DataMart.Lib.PropertyData> CurrentProperties;

        public DataSourceEditor(Lpp.Dns.DataMart.Model.IModelMetadata modelData, IList<Lpp.Dns.DataMart.Lib.PropertyData> properties)
        {
            this.ModelData = modelData;
            this.CurrentProperties = properties;

            InitializeComponent();

            txtPort.KeyPress += (sender, e) => {
                if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            };
        }

        private void DataSourceEditor_Load(object sender, EventArgs e)
        {
            ModelData.SQlProviders.ForEach(p => cmbDataProvider.Items.Add(p));
            cmbDataProvider.SelectedIndex = 0;

            if (ModelData.SQlProviders.Any(p => p == Model.Settings.SQLProvider.ODBC))
            {
                try
                {
                    cmbDataSourceName.DataSource = Lpp.Dns.DataMart.Client.Utils.OdbcUtil.GetAllDataSourceNames().Keys;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            //set to the current values
            foreach (var s in ModelData.Settings)
            {
                var setting = s;
                //only process the setting if it is a database setting
                if (Lpp.Dns.DataMart.Model.Settings.ProcessorSettings.IsDbSetting(setting.Key) == false)
                    continue;

                string value = CurrentProperties.Where(p => string.Equals(setting.Key, p.Name, StringComparison.OrdinalIgnoreCase)).Select(p => p.Value).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = setting.DefaultValue ?? string.Empty;
                }

                switch (setting.Key.ToUpper())
                {
                    case "DATAPROVIDER":
                        Lpp.Dns.DataMart.Model.Settings.SQLProvider sqlProvider;
                        if (Enum.TryParse<Lpp.Dns.DataMart.Model.Settings.SQLProvider>(value, out sqlProvider))
                        {
                            cmbDataProvider.SelectedItem = sqlProvider;
                        }
                        break;
                    case "SERVER":
                        txtServer.Text = value;
                        break;
                    case "PORT":
                        int port;
                        if (int.TryParse(value, out port))
                        {
                            txtPort.Text = value;
                        }
                        break;
                    case "USERID":
                        txtUserID.Text = value;
                        break;
                    case "PASSWORD":
                        txtPassword.Text = value;
                        break;
                    case "DATABASE":
                        txtDatabase.Text = value;
                        break;
                    case "DATASOURCENAME":
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            cmbDataSourceName.SelectedItem = value;
                        }
                        break;
             
                    case "CONNECTIONTIMEOUT":
                        int connectionTimeout;
                        if (int.TryParse(value, out connectionTimeout))
                        {
                            txtConnectionTimeout.Value = connectionTimeout;
                        }
                        break;
                    case "COMMANDTIMEOUT":
                        int commandTimeout;
                        if (int.TryParse(value, out commandTimeout))
                        {
                            txtCommandTimeout.Value = commandTimeout;
                        }
                        break;
                }
            }
        }

        public IEnumerable<Lpp.Dns.DataMart.Lib.PropertyData> GetSettings()
        {            
            Lpp.Dns.DataMart.Model.Settings.SQLProvider sqlProvider = (Lpp.Dns.DataMart.Model.Settings.SQLProvider)cmbDataProvider.SelectedItem;
            bool isODBC = sqlProvider == Model.Settings.SQLProvider.ODBC;
            bool isOracle = sqlProvider == Model.Settings.SQLProvider.Oracle;

            List<Lpp.Dns.DataMart.Lib.PropertyData> settings = new List<DataMart.Lib.PropertyData>();
            settings.Add(new DataMart.Lib.PropertyData("DataProvider", sqlProvider.ToString()));
            settings.Add(new DataMart.Lib.PropertyData("Server", isODBC ? string.Empty : txtServer.Text.Trim()));
            settings.Add(new DataMart.Lib.PropertyData("Port", isODBC ? string.Empty : txtPort.Text.Trim()));
            settings.Add(new DataMart.Lib.PropertyData("UserID", isODBC ? string.Empty : txtUserID.Text.Trim()));
            settings.Add(new DataMart.Lib.PropertyData("Password", isODBC ? string.Empty : txtPassword.Text.Trim()));
            settings.Add(new DataMart.Lib.PropertyData("Database", isODBC ? string.Empty : txtDatabase.Text.Trim()));
            settings.Add(new DataMart.Lib.PropertyData("DataSourceName", isODBC ? cmbDataSourceName.SelectedItem.ToString() : string.Empty));
            settings.Add(new DataMart.Lib.PropertyData("ConnectionTimeout", txtConnectionTimeout.Value.ToString()));
            settings.Add(new DataMart.Lib.PropertyData("CommandTimeout", txtCommandTimeout.Value.ToString()));

            return settings;
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            var selectedProvider = (Model.Settings.SQLProvider)cmbDataProvider.SelectedItem;
            bool isOracle = selectedProvider == Model.Settings.SQLProvider.Oracle;

            string port = txtPort.Text.Trim();
            int number;
            if (!string.IsNullOrWhiteSpace(txtPort.Text.Trim()) && !int.TryParse(port, out number) && !isOracle)
            {
                MessageBox.Show("Please Enter Valid Port Number");
                return;
            }
            int connectionTimeout;
            if (!int.TryParse(txtConnectionTimeout.Value.ToString(), out connectionTimeout))
            {
                MessageBox.Show("Please Enter Valid Connection Timeout Value in seconds.");
                return;
            }
            int commandTimeout;
            if (!int.TryParse(txtCommandTimeout.Value.ToString(), out commandTimeout))
            {
                MessageBox.Show("Please Enter Valid Command Timeout Value in seconds.");
                return;
            }
            string server = txtServer.Text;
            if (string.IsNullOrWhiteSpace(txtServer.Text.Trim()) && isOracle)
            {
                MessageBox.Show("Please Enter Valid Server.");
                return;
            }
            string database = txtDatabase.Text;
            if (string.IsNullOrWhiteSpace(txtDatabase.Text.Trim()) && isOracle)
            {
                MessageBox.Show("Please Enter Valid Oracle SID.");
                return;
            }
            string userID = txtUserID.Text;
            string password = txtPassword.Text;
            if (string.IsNullOrWhiteSpace(userID) && !string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please Enter Valid User ID.");
                    return;
            }
            if (!string.IsNullOrWhiteSpace(userID) && string.IsNullOrWhiteSpace(password) && isOracle && (userID!="/"))
            {
                MessageBox.Show("Please Enter Valid Password.");
                return;
            }
            string dsn = (cmbDataSourceName.SelectedItem ?? string.Empty).ToString();

            Model.Settings.SQLProvider sqlProvider = (Model.Settings.SQLProvider)Enum.Parse(typeof(Model.Settings.SQLProvider), cmbDataProvider.SelectedItem.ToString(), true);

            Dictionary<string, object> settings = new Dictionary<string, object>();
            settings.Add("DataProvider", cmbDataProvider.SelectedItem.ToString());
            settings.Add("Server", server);
            settings.Add("Port", port);
            settings.Add("Database", database);
            settings.Add("UserID", userID);
            settings.Add("Password", password);
            settings.Add("ConnectionTimeout", connectionTimeout);
            settings.Add("CommandTimeout", commandTimeout);

            if (sqlProvider == Model.Settings.SQLProvider.ODBC)
            {
                settings.Add("DataSourceName",dsn);
            }

            try
            {
                var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                Assembly assembly = (from a in currentAssemblies where a.GetName().Name == "Lpp.Dns.DataMart.Client.Database" select a).FirstOrDefault();
                if(assembly == null)
                {
                    assembly = Assembly.LoadFrom("Lpp.Dns.DataMart.Client.Database.dll");
                }

                Type dbTesterType = assembly.GetType("Lpp.Dns.DataMart.Client.Database.ConnectionTester");
                MethodInfo methodInfo = dbTesterType.GetMethod("Test");

                var classInstance = Activator.CreateInstance(dbTesterType, null);

                object[] parameters = new[] { settings };
                var result = methodInfo.Invoke(classInstance, parameters);

                if (Convert.ToBoolean(result))
                {
                    MessageBox.Show("Connection Successful", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }else
                {
                    MessageBox.Show("Unable to connect, please verify settings.", "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch (TargetInvocationException ex)
            {
                if (!ex.InnerException.IsNull() && !ex.InnerException.Message.IsNullOrWhiteSpace())
                    MessageBox.Show(ex.InnerException.Message, "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show(ex.Message, "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void cmbDataProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedProvider = (Model.Settings.SQLProvider)cmbDataProvider.SelectedItem;
            pnlDirect.Visible = selectedProvider != Model.Settings.SQLProvider.ODBC;
            pnlODBC.Visible = selectedProvider == Model.Settings.SQLProvider.ODBC;            
        }
        
    }
}
