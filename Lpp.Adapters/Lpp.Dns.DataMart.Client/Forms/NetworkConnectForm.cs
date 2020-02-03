using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Lpp.Dns.DataMart.Model;
using Lpp.Dns.DataMart.Lib;
using Lpp.Dns.DataMart.Client.Utils;
using System.Security;
using log4net;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using Lpp;
using Lpp.Dns.DataMart.Lib.Classes;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using Lpp.Dns.DataMart.Lib.Utils;

namespace Lpp.Dns.DataMart.Client
{
    public partial class NetworkConnectForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public delegate void NetworkSettingChangedEventHandler(object sender, NetWorkSetting ns);
        public event NetworkSettingChangedEventHandler NetworkSettingChanged;

        #region Properties

        public Dictionary<string, string> StartupParamsDict { get; set; }

        #endregion

        #region Constructors

        public NetworkConnectForm(Dictionary<string, string> startupParamsDict)
        {
            InitializeComponent();
            StartupParamsDict = startupParamsDict;
        }

        #endregion

        #region Event Handlers

        private void NetworkConnectForm_Load(object sender, EventArgs e)
        {
            try
            {
                lblStatusMessage.Text = string.Empty;

                var certs = LoadCerts();
                cbCertificates.DataSource = certs;

                if (StartupParamsDict != null)
                {
                    txtServiceUrl.Text = StartupParamsDict.ContainsKey("serviceUrl") ? StartupParamsDict["serviceUrl"] : string.Empty;
                    txtNetworkname.Text = StartupParamsDict.ContainsKey("networkName") ? StartupParamsDict["networkName"] : string.Empty;
                    txtUsername.Text = StartupParamsDict.ContainsKey("username") ? StartupParamsDict["username"] : string.Empty;
                    txtPassword.Text = string.Empty;
                }
                else
                {
                    txtServiceUrl.Text = string.Empty;
                    txtNetworkname.Text = string.Empty;
                    txtUsername.Text = string.Empty;
                    txtPassword.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Get the network setting for this service url if it exists already.
                NetWorkSetting ns = null;
                try
                {
                    ns = (from n in Configuration.Instance.NetworkSettingCollection.NetWorkSettings.ToArray(typeof(NetWorkSetting)) as NetWorkSetting[]
                          where n.HubWebServiceUrl == txtServiceUrl.Text
                          select n).FirstOrDefault();
                }
                catch
                {
                    Configuration.CreateNewNetworkSettingsFile();
                }

                // If not, create one.
                if (ns == null)
                {
                    ns = new NetWorkSetting();
                    ns.HubWebServiceUrl = txtServiceUrl.Text;
                    Configuration.Instance.AddNetworkSetting(ns);
                }
                else
                {
                    if (MessageBox.Show("This will override your existing configuration for this network.\nAre you sure you want to do that?", "Override Network Configuration?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                    {
                        this.Close();
                        return;
                    }
                }

                ns.Username = txtUsername.Text;
                ns.EncryptedPassword = txtPassword.Text;
                ns.NetworkName = txtNetworkname.Text;
                var selectedCert = cbCertificates.SelectedItem as Cert;
                ns.X509CertThumbprint = selectedCert == null ? null : selectedCert.Thumbprint;
                ns.GetDataMartsByUser();
                Configuration.Instance.LoadModels(ns);
                CredentialManager.SaveCredential(ns.CredentialKey, ns.Username, ns.Password);

                // Get the configuration for all models for all datamarts at this service url.
                var dmc = DnsServiceManager.GetDataMartModelConfigurations(ns);

                // Find the model processor in the existing network if exists. If not, create it.
                XmlSerializer serializer = new XmlSerializer(typeof(ModelProperties));
                foreach (var c in dmc)
                {
                    var d = ns.DataMartList.FirstOrDefault( dm => dm.DataMartId == c.DataMartId );
                    var mp = d.ModelList.FirstOrDefault( m => m.ModelId == c.ModelId && m.ProcessorId == c.ModelProcessorId );

                    if (d != null && mp != null && !string.IsNullOrEmpty(c.Properties))
                        using (XmlTextReader reader = new XmlTextReader(new MemoryStream(new System.Text.UTF8Encoding().GetBytes(c.Properties))))
                        {
                            d.AllowUnattendedOperation = c.UnattendedMode != Lpp.Dns.DataMart.Lib.Classes.UnattendedMode.NoUnattendedOperation;
                            d.NotifyOfNewQueries = c.UnattendedMode == Lpp.Dns.DataMart.Lib.Classes.UnattendedMode.NotifyOnly;
                            d.ProcessQueriesAndNotUpload = c.UnattendedMode == Lpp.Dns.DataMart.Lib.Classes.UnattendedMode.ProcessNoUpload;
                            d.ProcessQueriesAndUploadAutomatically = c.UnattendedMode == Lpp.Dns.DataMart.Lib.Classes.UnattendedMode.ProcessAndUpload;
                            var p = new List<PropertyData>();
                            p.AddRange(((ModelProperties)serializer.Deserialize(reader)).Properties.Where(prop => prop.Name != "ConfirmPassword"));
                            mp.Properties = p;

                            if (mp.ProcessorId != Guid.Empty)
                            {
                                try
                                {
                                    //TODO: confirm the latest package exists for the model/processor combination

                                    var packageIdentifier = DnsServiceManager.GetRequestTypeIdentifier(ns, mp.ModelId, mp.ProcessorId);
                                    if (packageIdentifier != null && !File.Exists(Path.Combine(Configuration.PackagesFolderPath, packageIdentifier.Identifier + "." + packageIdentifier.Version + ".zip")))
                                    {
                                        DnsServiceManager.DownloadPackage(ns, packageIdentifier);
                                    }

                                    //string processorPath, className;
                                    //ProcessorManager.FindProcessor(mp.ModelId, mp.ProcessorId, out processorPath, out className);
                                    //mp.ProcessorPath = processorPath;
                                    //mp.ClassName = className;
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Unable to load processor.", ex);
                                }
                            }
                        }

                }

                Configuration.SaveNetworkSettings();

                if (NetworkSettingChanged != null)
                    NetworkSettingChanged(this, ns);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        
        #region private methods

        private bool IsValidNetworkSetting()
        {
            bool isValid = true;

            try
            {
                if (string.IsNullOrEmpty(txtNetworkname.Text))
                {
                    MessageBox.Show("Please enter a Network name");
                    isValid = false;
                }

                if (isValid && string.IsNullOrEmpty(txtUsername.Text))
                {
                    MessageBox.Show("Please enter a Username");
                    isValid = false;
                }

                if (isValid && string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Please enter a password");
                    isValid = false;
                }

                return isValid;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        class Cert
        {
            public string Name { get; set; }
            public string Thumbprint { get; set; }
        }

        private IList<Cert> LoadCerts()
        {
            var store = new X509Store( StoreName.My, StoreLocation.CurrentUser );
            try
            {
                store.Open( OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly | OpenFlags.MaxAllowed );
                return store.Certificates.Cast<X509Certificate2>().Select( c => new Cert
                {
                    Name = c.FriendlyName.NullOrSpace() ? c.SubjectName.Name : c.FriendlyName,
                    Thumbprint = c.Thumbprint
                } )
                .ToList();
            }
            catch
            {
                return Enumerable.Empty<Cert>().ToList();
            }
            finally
            {
                store.Close();
            }
        }
    }
}
