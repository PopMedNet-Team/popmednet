using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using IWshRuntimeLibrary;
using log4net;
using Lpp.Dns.DataMart.Client.Controls;
using Lpp.Dns.DataMart.Client.Utils;
using Lpp.Dns.DataMart.Lib;
using Lpp.Dns.DataMart.Lib.Classes;
using Lpp.Dns.DataMart.Lib.RequestQueue;
using Lpp.Dns.DataMart.Lib.Utils;
using Lpp.Dns.DataMart.Model;

namespace Lpp.Dns.DataMart.Client
{
    public partial class RequestListForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDisposable _autoRefresh;

        private ImageList _tabIcons;
        const string TabIcon_NewRequests = "NewRequests";
        const string TabIcon_Regular = "Regular";
        const string TabIcon_Broken = "Broken";

        #region System Event Handlers

        private bool minimizedToTray;

        /// <summary>
        /// Code to show single instance.
        /// </summary>
        /// <param name="message"></param>
        protected override void WndProc(ref Message message)
        {
            try
            {
                if ( message.Msg == SingleInstanceChecker.WM_SHOWFIRSTINSTANCE )
                {
                    ShowWindow();
                    ShowConnectToNetworkDialog(StartupParams.GetAddNetworkStartupParamsDictionary(StartupParams.ReadStartupParamsFromFile(true)));
                }
                base.WndProc( ref message );
            }
            catch ( Exception ex )
            {
                log.Error( ex );
                // Log it and keep going.
            }
        }

        public void ShowWindow()
        {
            try
            {
                if (minimizedToTray)
                {
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    minimizedToTray = false;
                }
                else
                {
                    WinApi.ShowToFront(this.Handle);
                }
            }
            catch (Exception ex)
            {
                log.Error( ex );
            }
        }

        protected void ShowConnectToNetworkDialog(Dictionary<string, string> AddNetworkStartupParamsDict)
        {
            // Do not show the dialog unless a serviceUrl has been provided!
            if (AddNetworkStartupParamsDict != null && AddNetworkStartupParamsDict.ContainsKey("serviceUrl"))
            {
                // Use the retrieved startup parameters to generate a "Connect to Network" popup window.
                NetworkConnectForm f = new NetworkConnectForm(AddNetworkStartupParamsDict);
                f.NetworkSettingChanged += ConfigurationChanged_EventHandler;
                f.ShowDialog();
            }
        }

        protected override void OnClosed( EventArgs e )
        {
            base.OnClosed( e );
            SystemTray.HideSystemTrayIcon();

            _autoRefresh.CallDispose();
            _tabIcons.CallDispose();
        }

        #endregion

        #region Constructor

        public RequestListForm(Dictionary<string, string> AddNetworkStartupParamsDict)
        {
            // If the DMC was run with the /addNetwork command line parameter,
            // pop up the "Connect to Network" dialog and perform automatic
            // DataMart configuration BEFORE starting to load the main form,
            // so that the configuration changes will take effect immediately.
            ShowConnectToNetworkDialog(AddNetworkStartupParamsDict);

            InitializeComponent();
            splash.Dock = tabs.Dock = noNetworks.Dock = DockStyle.Fill;

            tabs.ImageList = _tabIcons = new ImageList { Images = 
            {
                { TabIcon_Regular, Properties.Resources.NotifyIconIdle },
                { TabIcon_NewRequests, Properties.Resources.NotifyIconNewQuery },
                { TabIcon_Broken, Properties.Resources.NotifyIconBusy }
            } };
            tabs.SelectedIndexChanged += ( _, __ ) => ToggleDetailsButton();
        }

        #endregion

        #region Event Handlers

        public void RefreshAutoResponse()
        {
            _autoRefresh.CallDispose();
            
            var refreshRate = TimeSpan.FromMilliseconds(Properties.Settings.Default.RefreshRate);
            _autoRefresh = Observable
                .Timer(DateTimeOffset.Now.Add(refreshRate), refreshRate, new ControlScheduler(this))
                .Do(_ => { if (cbAutoRefresh.Checked) ReloadAllLists(); })
                .LogExceptions(log.Error)
                .Retry()
                .Subscribe();
        }

        private void RequestListForm_Load(object sender, EventArgs e)
        {
            try
            {
                splash.BringToFront();
                SystemTray.DisplaySystemTrayIcon();
                SystemTray.NotifyIconPicked += notifyIcon_Picked;

                DisplayInformationalStatus( "Initializing", TextColor.Normal, toolStripLoginStatus );
                DisplayInformationalStatus("Identifying Network Settings. Please Wait...", TextColor.Normal, toolStripStatusNetworkConnectivityStatus);

                try
                {
                    Configuration configuration = Configuration.Instance;
                }
                catch (NetworkSettingsFileNotFound ex)
                {
                    Configuration.CreateNewNetworkSettingsFile();
                    DisplayInformationalStatus(ex.Message, TextColor.Alert, toolStripStatusNetworkConnectivityStatus);
                }

                cbRunAtWindowsStartup.Checked = Properties.Settings.Default.RunAtStartup;
                cbAutoRefresh.Checked = Properties.Settings.Default.AutoRefresh;

                // Delegate the rest the of time consuming (but non-UI) startup tasks to a worker thread,
                // so the main screen get show up quickly.
                startupWorker.RunWorkerAsync();

                DisplayInformationalStatus("Connecting", TextColor.Normal, toolStripLoginStatus);
                DisplayInformationalStatus(string.Format("Connecting to {0} Network(s)", Configuration.Instance.NetworkSettingCollection.NetWorkSettings.Count), TextColor.Normal, toolStripStatusNetworkConnectivityStatus);

            }
            catch (Exception ex)
            {
                log.Error( ex );
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Perform the remaining time consuming UI tasks here since this is part of the main UI thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startupWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            splash.Hide();
            ToggleNoNetworksMessage();

            foreach ( NetWorkSetting ns in Configuration.Instance.NetworkSettingCollection.NetWorkSettings ) CreateList( ns );
            
            var refreshRate = TimeSpan.FromMilliseconds( Properties.Settings.Default.RefreshRate );
            _autoRefresh = Observable
                .Timer( DateTimeOffset.Now.Add( refreshRate ), refreshRate, new ControlScheduler( this ) )
                .Do( _ => { if ( cbAutoRefresh.Checked ) ReloadAllLists(); } )
                .LogExceptions( log.Error )
                .Retry()
                .Subscribe();

            GC.KeepAlive( AutoProcessingWorker.Instance );
        }

        private void ToggleNoNetworksMessage()
        {
            noNetworks.BringToFront();
            noNetworks.Visible = Configuration.Instance.NetworkSettingCollection.NetWorkSettings.Count == 0;
        }

        void ReloadAllLists() { foreach ( var l in AllLists() ) l.ReloadWithNetworkCheck(); }

        private void ToggleDetailsButton()
        {
            var list = CurrentList();
            btnViewDetail.Enabled = list != null && list.SelectedRequest != null;
        }

        RequestListGridView CreateList( NetWorkSetting ns )
        {
            var existing = AllLists().FirstOrDefault( l => l.Network == ns );
            if ( existing != null ) return existing;

            var g = new RequestListGridView( ns )
            {
                Dock = DockStyle.Fill,
                Filter = ns.Filter,
                PageSize = ns.PageSize,
                SortColumn = ns.Sort,
                IsSortAscending = ns.SortAscending
            };
            var tab = new TabPage { Text = ns.NetworkName, Controls = { g } };
            tabs.TabPages.Add( tab );
            tab.ImageKey = TabIcon_Regular;

            g.RequestRowDoubleClick += rlgvRequestList_RequestRowDoubleClick;
            g.ReloadStarted += ( _, __ ) => SetReloadingStatusText();
            g.SelectedRequestChanged += ( _, __ ) => ToggleDetailsButton();
            g.Reloaded += ( _, __ ) => { SetReloadingStatusText(); ToggleDetailsButton(); SetNewRequestsIndicator( tab ); };
            g.ReloadFailed += ( _, __ ) => { SetReloadingStatusText(); ToggleDetailsButton(); SetNewRequestsIndicator( tab ); };
            g.NewRequestsAvailableChanged += ( _, __ ) => { SetNewRequestsIndicator( tab ); ResetNewRequestsIndicator(); };

            EventHandler saveGridSettings = ( _, __ ) =>
            {
                ns.PageSize = g.PageSize;
                ns.Filter = g.Filter;
                ns.SortAscending = g.IsSortAscending;
                ns.Sort = g.SortColumn;
                Configuration.SaveNetworkSettings();
            };
            g.FilterChanged += saveGridSettings;
            g.PageSizeChanged += saveGridSettings;
            g.SortModeChanged += saveGridSettings;

            SetNewRequestsIndicator( tab );
            return g;
        }

        void SetNewRequestsIndicator( TabPage tab )
        {
            var g = tab.Controls[0] as RequestListGridView;
            
            tab.ImageKey = g.Network.NetworkStatus == Util.ConnectionOKStatus 
                ? g.NewRequestsAvailable ? TabIcon_NewRequests : TabIcon_Regular
                : TabIcon_Broken;

            tab.ToolTipText = g.Network.NetworkStatus == Util.ConnectionOKStatus 
                ? g.NewRequestsAvailable ? "There are new requests available on this network" : ""
                : "There is a problem with this connection";
        }

        void tabs_SelectedIndexChanged( object sender, EventArgs e )
        {
            ResetNewRequestsIndicator();
        }

        void ResetNewRequestsIndicator()
        {
            var l = CurrentList();
            if ( l != null ) l.NewRequestsAvailable = false;
        }

        RequestListGridView CurrentList()
        {
            var res = tabs.SelectedTab == null ? null : (tabs.SelectedTab.Controls[0] as RequestListGridView);
            return res ?? AllLists().FirstOrDefault();
        }

        void SetReloadingStatusText()
        {
            var isReloading = AllLists().Any( l => l.IsReloading );
            DisplayInformationalStatus( isReloading ? "Fetching requests from the network..." : "Ready", 
                TextColor.Normal, toolStripStatusNetworkConnectivityStatus );
        }

        IEnumerable<RequestListGridView> AllLists() { return tabs.TabPages.Cast<TabPage>().Select( p => p.Controls[0] ).OfType<RequestListGridView>(); }
         
        private void startupWorker_DoWork(object sender, DoWorkEventArgs args)
        {
            try
            {
                CreateShortcutinStartup();
                TestConnections();
            }
            catch (Exception ex)
            {
                log.Error( ex );
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbAutoRefresh_CheckedChanged( object sender, EventArgs e )
        {
            Properties.Settings.Default.AutoRefresh = cbAutoRefresh.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbRunAtWindowsStartup_CheckedChanged( object sender, EventArgs e )
        {
            CreateShortcutinStartup();
        }

        /// <summary>
        /// User double clicks on a row in the RequestListGridView.
        /// Show the corresponding request detail.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rlgvRequestList_RequestRowDoubleClick(object sender, RequestDetailEventArgs e)
        {
            try
            {
                ShowRequestDetailDialog(e.Network, e.RequestID, e.DataMartID);
            }
            catch (Exception ex)
            {
                log.Error( ex );
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// User clicks on the View Detail button.
        /// Show the corresponding request detail.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewDetail_Click(object sender, EventArgs e)
        {
            try
            {
                var list = CurrentList();
                var request = list == null ? null : list.SelectedRequest;
                if ( request != null )
                {
                    ShowRequestDetailDialog( list.Network, request.ID, request.DataMartID );
                }
            }
            catch (Exception ex)
            {
                log.Error( ex );
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// User closes but not exits the application.
        /// Minimize DataMart Client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                minimizedToTray = true;
            }
            catch (Exception ex)
            {
                log.Error( ex );
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                NetworkListForm f = new NetworkListForm();
                f.ConfigurationChanged += ConfigurationChanged_EventHandler;
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error( ex );
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Refresh_EventHandler(object sender, EventArgs e)
        {
            try
            {
                var l = CurrentList();
                if ( l != null ) l.Reload();
            }
            catch (Exception ex)
            {
                log.Error( ex );
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExitApplication_EventHandler(object sender, EventArgs e)
        {
            try
            {
                log.Info("Logged out of DataMart Client.");
                foreach (NetWorkSetting ns in Configuration.Instance.NetworkSettingCollection.NetWorkSettings)
                {
                    if (ns.SftpClient != null)
                        ns.SftpClient.Dispose();
                }
                SystemTray.HideSystemTrayIcon();
                Application.DoEvents();
                Application.Exit();
            }
            catch (Exception ex)
            {
                log.Error( ex );
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ConfigurationChanged_EventHandler(object sender, NetWorkSetting networkSetting)
        {
            // Invalidate the request cache for network when things change on the connection since the user name or URL may have changed.  We could be more selective in when we need to invalidate, but for now this will do.
            RequestCache.ForNetwork(networkSetting).Invalidate();
            if (!this.Visible)
                return;

            try
            {
                TestConnections();

                foreach ( var p in tabs.TabPages.Cast<TabPage>().ToList() )
                {
                    var l = p.Controls[0] as RequestListGridView;
                    if ( !Configuration.Instance.NetworkSettingCollection.NetWorkSettings.Contains( l.Network ) )
                    {
                        l.Dispose();
                        tabs.TabPages.Remove( p );
                    }
                    else
                    {
                        l.OnConfigurationChanged();
                    }
                }
                Configuration.Instance.NetworkSettingCollection.NetWorkSettings
                    .Cast<NetWorkSetting>()
                    .Except( AllLists().Select( l => l.Network ) )
                    .ForEach( ns => CreateList( ns ) );

                ToggleNoNetworksMessage();
                ReloadAllLists();
            }
            catch (Exception ex)
            {
                log.Error( ex );
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TestConnections()
        {
            int numConnectionsOk = DnsServiceManager.TestConnections(Configuration.Instance.NetworkSettingCollection.AsEnumerable());
            this.Invoke( new Action( () =>
            {
                if (numConnectionsOk > 0)
                {
                    DisplayInformationalStatus(string.Format("Successfully connected to {0} out of {1} Network(s)", numConnectionsOk, Configuration.Instance.NetworkSettingCollection.NetWorkSettings.Count), TextColor.Normal, toolStripStatusNetworkConnectivityStatus);
                    DisplayInformationalStatus(string.Format("Connected", numConnectionsOk, Configuration.Instance.NetworkSettingCollection.NetWorkSettings.Count), TextColor.Normal, toolStripLoginStatus);
                    DisplayInformationalStatus("Ready", TextColor.Normal, toolStripStatusNetworkConnectivityStatus);
                }
                else
                {
                    int numNetworks = Configuration.Instance.NetworkSettingCollection.NetWorkSettings.Count;

                    if (numNetworks > 0)
                        DisplayInformationalStatus(string.Format("Failed to connect to all {0} Network(s). Check Settings.", Configuration.Instance.NetworkSettingCollection.NetWorkSettings.Count), TextColor.Error, toolStripStatusNetworkConnectivityStatus);
                    else
                        DisplayInformationalStatus(string.Format("No Network configured. Check Settings.", Configuration.Instance.NetworkSettingCollection.NetWorkSettings.Count), TextColor.Error, toolStripStatusNetworkConnectivityStatus);

                    DisplayInformationalStatus("Disconnected", TextColor.Alert, toolStripLoginStatus);
                }
            } ) );
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Display informational status 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Type"></param>
        /// <param name="sender"></param>
        private void DisplayInformationalStatus(string Message, TextColor Type, ToolStripStatusLabel sender)
        {
            Color MessageColor;
            switch (Type)
            {
                case TextColor.Normal:
                    MessageColor = Color.Green;
                    break;
                case TextColor.Alert:
                    MessageColor = Color.Navy;
                    break;
                case TextColor.Error:
                    MessageColor = Color.Red;
                    break;
                default:
                    MessageColor = Color.Green;
                    break;
            }

            UpdateStatusLabelText(Message, MessageColor, sender /*, toolStripContainer1*/);
        }

        private void UpdateStatusLabelText(string statusText, Color messageColor, ToolStripStatusLabel sender/*,ToolStripContainer senderParent*/)
        {
            sender.Text = statusText;
            sender.ForeColor = messageColor;
        }

        private void ShowRequestDetailDialog(NetWorkSetting ns, Guid id, Guid dataMartId)
        {
            var progress = new ProgressForm( "Loading Request Information", "Loading full Request information from the Network..." );
            RequestCache.ForNetwork( ns )
                .LoadRequest( id, dataMartId )
                .TakeUntil( progress.ShowAndWaitForCancel( this ) )
                .ObserveOn( this )
                .Finally( progress.Dispose )
                .Do( request =>
                {
                    using (var f = new RequestDetailForm(request))
                    {
                        f.FormClosed += Refresh_EventHandler;
                        f.Shown += (_, __) => progress.Dispose();
                        f.ShowDialog(this);
                    }
                } )
                .LogExceptions( log.Error )
                .Catch( ( IncompatibleProcessorException e ) => MessageBox.Show( e.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information ) )
                .Catch( ( CannotLoadProcessorException e ) => MessageBox.Show( e.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information ) )
                .Catch( (Exception e) => MessageBox.Show( string.Join( Environment.NewLine, OpResult.FromException( e ).ErrorMessages ), "Unexpected error", MessageBoxButtons.OK, MessageBoxIcon.Error ) )
                .Subscribe();
        }


        /// <summary>
        /// Create DM Client shortcut in startup folder for logged in (windows) user.
        /// </summary>
        private void CreateShortcutinStartup()
        {
            try
            {
                Properties.Settings.Default.RunAtStartup = cbRunAtWindowsStartup.Checked;
                Properties.Settings.Default.Save();

                string CurrentUserDir = Environment.GetFolderPath( Environment.SpecialFolder.Startup );
                string linkName = "DataMart Client";
                if ( cbRunAtWindowsStartup.Checked )
                {
                    IWshShortcut shortcut = (IWshShortcut)new WshShellClass().CreateShortcut( Path.Combine( CurrentUserDir, linkName + ".lnk" ) );
                    shortcut.TargetPath = Application.ExecutablePath;
                    shortcut.Description = "DataMart Client";
                    shortcut.Save();
                }
                else
                {
                    System.IO.File.Delete(CurrentUserDir + "\\" + linkName + ".lnk");
                }
            }
            catch (Exception ex)
            {
                log.Error( ex );
                // Log it and keep going.
            }
        }


        /// <summary>
        /// Event handler for MouseDoubleClick event on notifyIcon -- occurs when user double clicks the system tray icon
        /// </summary>
        internal void notifyIcon_Picked(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void openSettings_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            settingsToolStripMenuItem_Click( this, EventArgs.Empty );
        }

        #endregion

        private enum TextColor
        {
            Normal = 0,
            Alert = 1,
            Error = 2
        }

        private void lblAbout_Click(object sender, EventArgs e)
        {
            try
            {
                AboutForm aboutDialog = new AboutForm(this);

                aboutDialog.LogFilePath = (Properties.Settings.Default.LogFilePath == null ? string.Empty : Properties.Settings.Default.LogFilePath);
                aboutDialog.LogLevel = (Properties.Settings.Default.LogLevel == null ? string.Empty : Properties.Settings.Default.LogLevel);
                aboutDialog.DataMartClientId = (Properties.Settings.Default.DataMartClientId == null ? string.Empty : Properties.Settings.Default.DataMartClientId);
                aboutDialog.ShowDialog();
                Properties.Settings.Default.LogFilePath = aboutDialog.LogFilePath;
                Properties.Settings.Default.LogLevel = aboutDialog.LogLevel;
                Properties.Settings.Default.Save();
                Program.logWatcher.SetLogFilePath(aboutDialog.LogFilePath);
                Program.logWatcher.SetLogLevel(aboutDialog.LogLevel);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}