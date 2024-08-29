using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data.Common;
using System.IO;
using System.Reflection;


namespace Lpp.DisplaySecurity.Forms
{
    public partial class frmConnectionSettings : Form
    {
        List<ConnectionSetting> ListOfConnections = new List<ConnectionSetting>();
        string ConnectPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ConnectSettings.xml");

        public frmConnectionSettings()
        {
            
            InitializeComponent();
            XmlDocument doc = new XmlDocument();
            doc.Load(ConnectPath);
            XmlNodeList elemList = doc.GetElementsByTagName("Connection");
            foreach(XmlNode connection in elemList)
            {
                ListOfConnections.Add(new ConnectionSetting 
                { 
                    UID = connection["UID"].InnerText,
                    Password = connection["Password"].InnerText,
                    Server = connection["Server"].InnerText,
                    TrustedConnection = connection["TrustedConnection"].InnerText,
                    Database = connection["Database"].InnerText,
                    ConnectionTO = connection["ConnectionTO"].InnerText
                });
            }
            SavedConnections.DataSource = ListOfConnections;

            XDocument document = XDocument.Load(ConnectPath);
            SelectedConnection.Text = String.Format("Server: {0}, Database: {1}", 
                                                    document.Root.Element("DefaultConnection").Element("Server").Value,
                                                    document.Root.Element("DefaultConnection").Element("Database").Value
                                                    );
        }        

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            string QueryString;
            string TrustedConnection;
            if (TrustedConnectionChkBox.Checked)
            {
                TrustedConnection = "yes";
            }
            else
            {
                TrustedConnection = "no";
            }

            QueryString = String.Format("user id='{0}';password='{1}';server='{2}';Trusted_Connection='{3}';database='{4}'; connection timeout='{5}'",
                                                            UserIDTextBox.Text,
                                                            PasswordTextBox.Text,
                                                            ServerTextBox.Text,
                                                            TrustedConnection,
                                                            DatabaseTextBox.Text,
                                                            ConnectInput.Text);
            SqlConnection TestConnection = new SqlConnection(QueryString);
            TestConnection.Open();
            
            if (TestConnection.State == ConnectionState.Open)
            {
                ConnectionSuccess success = new ConnectionSuccess();
                success.ShowDialog();
            }
            else
            {
                ConnectionFailure fail = new ConnectionFailure();
                fail.ShowDialog();
            }
            TestConnection.Close();
        }

        private void btnSetDefault_Click(object sender, EventArgs e)
        {
            ComboBox currentComboBox = SavedConnections;
            string newUID = ((ConnectionSetting)currentComboBox.SelectedItem).UID;
            string newPassword = ((ConnectionSetting)currentComboBox.SelectedItem).Password;
            string newServer = ((ConnectionSetting)currentComboBox.SelectedItem).Server;
            string newTC = ((ConnectionSetting)currentComboBox.SelectedItem).TrustedConnection;
            string newDatabase = ((ConnectionSetting)currentComboBox.SelectedItem).Database;
            string newConnectionTO = ((ConnectionSetting)currentComboBox.SelectedItem).ConnectionTO;

            
            XDocument doc = XDocument.Load(ConnectPath);
            XElement defaultCon = doc.Root.Element("DefaultConnection");

            defaultCon.SetElementValue("UID", newUID);
            defaultCon.SetElementValue("Password", newPassword);
            defaultCon.SetElementValue("Server", newServer);
            defaultCon.SetElementValue("TrustedConnection", newTC);
            defaultCon.SetElementValue("Database", newDatabase);
            defaultCon.SetElementValue("ConnectionTO", newConnectionTO);
            doc.Save(ConnectPath);
            SelectedConnection.Text = String.Format("Server: {0}, Database: {1}", newServer, newDatabase);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string newServer = "";
            string newUID = "";
            string newPassword = "";
            string newDatabase = "";
            string newConnectionTO = "";
            string newTC = "";

            newServer = ServerTextBox.Text;
            newUID = UserIDTextBox.Text;
            newPassword = PasswordTextBox.Text;
            newDatabase = DatabaseTextBox.Text;
            newConnectionTO = ConnectInput.Text;
            if (TrustedConnectionChkBox.Checked)
            {
                newTC = "yes";
            }
            else
            {
                newTC = "no";
            }

            ListOfConnections.Add(new ConnectionSetting
            {
                UID = newUID,
                Password = newPassword,
                Server = newServer,
                TrustedConnection = newTC,
                Database = newDatabase,
                ConnectionTO = newConnectionTO
            });

            XDocument doc = XDocument.Load(ConnectPath);
            XElement connection = new XElement("Connection",
                                       new XElement("UID", newUID),
                                       new XElement("Password", newPassword),
                                       new XElement("Server", newServer),
                                       new XElement("TrustedConnection", newTC),
                                       new XElement("Database", newDatabase),
                                       new XElement("ConnectionTO", newConnectionTO)
                                       );
            doc.Root.Add(connection);
            doc.Save(ConnectPath);
            SavedConnections.DataSource = null;
            SavedConnections.DataSource = ListOfConnections;
            SavedConnections.SelectedItem = null;
            UserIDTextBox.Text = null;
            PasswordTextBox.Text = null;
            ServerTextBox.Text = null;
            DatabaseTextBox.Text = null;
            ConnectInput.Text = "0";
            TrustedConnectionChkBox.Checked = false;

        }

        private void SavedConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SavedConnections.DataSource != null && SavedConnections.SelectedItem != null)
            {
                ComboBox comboBox = ((ComboBox)sender);
                string UID = "";
                string Password = "";
                string Server = "";
                string TrustedConnection = "";
                string Database = "";
                string ConnectionTO = "";

                UID = ((ConnectionSetting)comboBox.SelectedItem).UID;
                Password = ((ConnectionSetting)comboBox.SelectedItem).Password;
                Server = ((ConnectionSetting)comboBox.SelectedItem).Server;
                TrustedConnection = ((ConnectionSetting)comboBox.SelectedItem).TrustedConnection;
                Database = ((ConnectionSetting)comboBox.SelectedItem).Database;
                ConnectionTO = ((ConnectionSetting)comboBox.SelectedItem).ConnectionTO;

                UserIDTextBox.Text = UID;
                PasswordTextBox.Text = Password;
                ServerTextBox.Text = Server;
                DatabaseTextBox.Text = Database;
                ConnectInput.Text = ConnectionTO;

                if (TrustedConnection == "yes")
                {
                    TrustedConnectionChkBox.Checked = true;
                }
                else
                {
                    TrustedConnectionChkBox.Checked = false;
                }
            }
            
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            string server = ((ConnectionSetting)SavedConnections.SelectedItem).Server;
            string database = ((ConnectionSetting)SavedConnections.SelectedItem).Database;
            string uid = ((ConnectionSetting)SavedConnections.SelectedItem).UID;
            string trustedconnection = ((ConnectionSetting)SavedConnections.SelectedItem).TrustedConnection;
            string connectionTO = ((ConnectionSetting)SavedConnections.SelectedItem).ConnectionTO;

            XDocument doc = XDocument.Load(ConnectPath);
            IEnumerable<XElement> Connections =
                    from Nodes in doc.Root.Elements("Connection")
                    where Nodes.Element("Server").Value == server
                    && Nodes.Element("Database").Value == database
                    && Nodes.Element("UID").Value == uid
                    && Nodes.Element("ConnectionTO").Value == connectionTO
                    && Nodes.Element("TrustedConnection").Value == trustedconnection
                    select Nodes;
            Extensions.Remove(Connections);
            doc.Save(ConnectPath);
            
            
            SavedConnections.SelectedItem = null;
            UserIDTextBox.Text = null;
            PasswordTextBox.Text = null;
            ServerTextBox.Text = null;
            DatabaseTextBox.Text = null;
            ConnectInput.Text = "0";
            TrustedConnectionChkBox.Checked = false;

            XmlDocument newSavedConnections = new XmlDocument();
            newSavedConnections.Load(ConnectPath);
            XmlNodeList elemList = newSavedConnections.GetElementsByTagName("Connection");
            ListOfConnections.Clear();
            foreach (XmlNode connection in elemList)
            {
                ListOfConnections.Add(new ConnectionSetting
                {
                    UID = connection["UID"].InnerText,
                    Password = connection["Password"].InnerText,
                    Server = connection["Server"].InnerText,
                    TrustedConnection = connection["TrustedConnection"].InnerText,
                    Database = connection["Database"].InnerText,
                    ConnectionTO = connection["ConnectionTO"].InnerText
                });
            }
            SavedConnections.DataSource = null;
            SavedConnections.DataSource = ListOfConnections;

        }

        private void TrustedConnectionChkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (TrustedConnectionChkBox.Checked)
            {
                UserIDTextBox.Text = "";
                UserIDTextBox.ReadOnly = true;
                PasswordTextBox.Text = "";
                PasswordTextBox.ReadOnly = true;

            }
            else
            {
                UserIDTextBox.ReadOnly = false;
                PasswordTextBox.ReadOnly = false;
            }
        }

        private void SelectedConnection_TextChanged(object sender, EventArgs e)
        {
            
        }


    }
}
