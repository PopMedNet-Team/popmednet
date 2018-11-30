using System;
using System.Reactive.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using Lpp.Dns.DataMart.Client.Utils;
using Lpp.Dns.DataMart.Lib;
using Lpp.Dns.DataMart.Lib.Classes;
using Lpp.Dns.DataMart.Lib.RequestQueue;
using log4net;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive;
using Lpp.Dns.DTO.DataMartClient;

namespace Lpp.Dns.DataMart.Client.Controls
{
    public partial class RequestListGridView : UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(RequestListGridView));

        private static readonly IList<DateFilterItem> _dates = new[]
        {
            new DateFilterItem( "30 Days", 30 ),
            new DateFilterItem( "90 Days", 90 ),
            new DateFilterItem( "180 Days", 180 ),
            new DateFilterItem( "1 Year", 365 ),
            new DateFilterItem( "Custom" )
        };

        private static readonly object[] _pageSizes = new[] { "10", "25", "50" };

        static readonly ConcurrentDictionary<string, Func<HubRequest, object>> _sortExpressionsCache = new ConcurrentDictionary<string, Func<HubRequest, object>>();

        public event EventHandler PageSizeChanged;
        public event EventHandler SortModeChanged;
        public event EventHandler<RequestDetailEventArgs> RequestRowDoubleClick;
        public event EventHandler FilterChanged;
        public event EventHandler ReloadStarted;
        public event EventHandler Reloaded;
        public event EventHandler ReloadFailed;
        public event EventHandler NewRequestsAvailableChanged;

        public event EventHandler SelectedRequestChanged
        {
            add { dgvRequestList.SelectionChanged += value; }
            remove { dgvRequestList.SelectionChanged -= value; }
        }

        public NetWorkSetting Network { get; private set; }
        public bool IsReloading
        {
            get
            {
                return _currentReload != null;
            }
        }

        private bool _freezeChangeFilter = false;
        private RequestFilter _filter;
        public RequestFilter Filter
        {
            get
            {
                return _filter;
            }

            set
            {
                _filter = value;
                if (!_freezeChangeFilter)
                {
                    FilterChanged.Raise(this);
                    SetFilterConrols();
                    Reload();
                }
            }
        }

        private int Page { get; set; }
        private int PageCount { get; set; }

        public void ChangeFilter(Action<IRequestFilter> f)
        {
            IRequestFilter o = Filter;
            f(o);
            Filter = (RequestFilter)o;
        }

        private RequestSortColumn? _sortColumn;
        private bool? _isSortAscending = null;
        private int _pageSize = 1;
        private IDisposable _currentReload;
        private IDisposable _pageSizeReset;

        public int PageSize
        {
            get 
            {
                return _pageSize; 
            }
            set 
            { 
                if (_pageSize != value) 
                { 
                    _pageSize = value; 
                    cmbPageSize.Text = value.ToString(); 
                    Reload(); 
                }
            }
        }

        public RequestSortColumn? SortColumn 
        {
            get 
            {
                return _sortColumn; 
            } 
            set 
            {
                if (_sortColumn != value) 
                {
                    _sortColumn = value; 
                    Reload(); 
                }
            }
        }

        public bool? IsSortAscending 
        {
            get 
            {
                return _isSortAscending; 
            } 
            set 
            {
                if (_isSortAscending != value) 
                {
                    _isSortAscending = value; 
                    Reload(); 
                }
            }
        }

        public RequestListRow SelectedRequest
        {
            get
            {
                var sels = dgvRequestList.SelectedRows;
                return sels.Count == 0 ? null : sels[0].DataBoundItem as RequestListRow;
            }
            set
            {
                if (dgvRequestList.Rows.Count == 0) return;
                dgvRequestList.Rows[0].Selected = false;
                if (value != null)
                {
                    var row = dgvRequestList.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.DataBoundItem == value);
                    if (row != null) row.Selected = true;
                }
            }
        }

        public int RowsShowing
        {
            get
            {
                return dgvRequestList.RowCount;
            }
        }

        private DateTime _lastNewRequestsLookup = DateTime.UtcNow;
        private bool _newRequestsAvailable = false;
        public bool NewRequestsAvailable
        {
            get { return _newRequestsAvailable; }
            set { if (_newRequestsAvailable != value) { _newRequestsAvailable = value; NewRequestsAvailableChanged.Raise(this); } }
        }

        private DateTime _lastInvisibleRequestsLookup = DateTime.UtcNow;
        private bool _invisibleRequestsAvailable = false;
        public bool InvisibleRequestsAvailable
        {
            get { return _invisibleRequestsAvailable; }
            set { if (_invisibleRequestsAvailable != value) { _invisibleRequestsAvailable = value; ToggleInvisibleRequestsMessage(); } }
        }

        void ToggleInvisibleRequestsMessage()
        {
            invisibleRequestsWarning.Visible = InvisibleRequestsAvailable;
        }

        readonly AutoProcessor AutoProcessor;

        public RequestListGridView(NetWorkSetting ns) : this()
        {
            Network = ns;

            AutoProcessor = new AutoProcessor(Network);
        }

        public RequestListGridView()
        {
            InitializeComponent();

            this.dgvRequestList.AutoGenerateColumns = false;
            this.colCreatedByUserName.Tag = RequestSortColumn.CreatedByUsername;
            this.colDataMartName.Tag = RequestSortColumn.DataMartName;
            this.colDueDate.Tag = RequestSortColumn.RequestDueDate;
            this.colId.Tag = RequestSortColumn.RequestId;
            this.colName.Tag = RequestSortColumn.RequestName;
            this.colPriority.Tag = RequestSortColumn.RequestPriority;
            this.colRequestTime.Tag = RequestSortColumn.RequestTime;
            this.colRespondedByUsername.Tag = RequestSortColumn.RespondedByUsername;
            this.colResponseTime.Tag = RequestSortColumn.ResponseTime;
            this.colStatus.Tag = RequestSortColumn.RequestStatus;
            this.colProject.Tag = RequestSortColumn.ProjectName;
            this.colModelType.Tag = RequestSortColumn.RequestModelType;
            this.colRequestType.Tag = RequestSortColumn.RequestType;
            this.colMSRequestID.Tag = RequestSortColumn.MSRequestID;
            this.connectionError.Dock = DockStyle.Fill;
            this.connectionError.Hide();

            cmbPageSize.Items.AddRange(_pageSizes);

            // The following hack is necessary, because the combobox with editable text field puts in the closest
            // matching value from its list and doesn't raise any events. For instance, if I say cmbPageSize.Text = "5",
            // it will eventually (no way to catch when exactly) set it to "50", because "50" is in its list,
            // but if I set it to "6", it will stay "6", will not change, because the list doesn't contain anything starting with "6".
            // Same goes for "2" -> "25" and "1" -> "10".
            // I could not find a way to battle this (no event get raised, not clear why this happens), so I'm employing
            // this hack: just reset the text to its proper value periodically.

            this.HandleCreated += (___, __) =>
                _pageSizeReset = Observable
                    .Interval(TimeSpan.FromSeconds(0.2))
                    .ObserveOn(this)
                    .Do(_ => { if (cmbPageSize.Text != _pageSize.ToString() && !cmbPageSize.DroppedDown && !cmbPageSize.Focused) cmbPageSize.Text = _pageSize.ToString(); })
                    .Catch()
                    .Subscribe();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            _pageSizeReset.CallDispose();
            base.OnHandleDestroyed(e);
        }

        public void OnConfigurationChanged(NetWorkSetting updatedNetworkSetting)
        {
            Network = updatedNetworkSetting;
            AutoProcessor.UpdateNetworkSetting(Network);
            ReloadDatamarts();
        }

        public void StopAutoprocessor()
        {
            AutoProcessor.Stop();
        }

        private void RequestListGridView_Load(object sender, EventArgs e)
        {
            Page = 0;
            lblTotalPages.Text = "";
            EnableDisablePageButtons();

            SetFilterConrols();
            Reload();
        }

        void SetFilterConrols()
        {
            var filter = Filter;
            _freezeChangeFilter = true;

            try
            {
                ReloadDatamarts();

                cmbStatus.SetList<Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus>(HubRequestStatus.All.Select(s => s.Code).ToArray(), filter.Statuses, d => HubRequestStatus.All.Single(s => s.Code == d).Name);
                
                SetStatusText();

                cmbDates.Items.Clear();

                _dates.ForEach(d => cmbDates.Items.Add(d));

                SetDateFilterSelectedItem(filter);

                cmbStatus.Enabled = cmbDataMarts.Enabled = cmbDates.Enabled = true;
            }
            finally
            {
                _freezeChangeFilter = false;
            }
        }

        private void SetDateFilterSelectedItem(RequestFilter filter)
        {
            if (filter.DateRange == DateRangeKind.RecentDays)
            {
                cmbDates.SelectedItem = _dates.FirstOrDefault(d => d.IsDaysBack && d.DaysBack == filter.RecentDaysToShow);
            }
            else
            {
                cmbDates.Items.Insert(0, new DateFilterItem(filter.FromDate ?? DateTime.UtcNow.AddDays(-30), filter.ToDate ?? DateTime.UtcNow));
                cmbDates.SelectedIndex = 0;
            }
        }

        private void ReloadDatamarts()
        {
            var filter = Filter;
            var dataMarts = Configuration.Instance.GetDataMartSelections(Network);
            cmbDataMarts.DataSource = dataMarts;

            // TODO: Make datamarts multiple selection
            var selectedDmId = filter.DataMartIds.EmptyIfNull().FirstOrDefault();
            cmbDataMarts.SelectedItem = dataMarts.FirstOrDefault(d => d.DataMartId == selectedDmId) ?? dataMarts.FirstOrDefault();
        }

        private void SetStatusText()
        {
            cmbStatus.Text = Filter.Statuses.NullOrEmpty() || Filter.Statuses.Length == HubRequestStatus.All.Count ? "All" : string.Join(", ", Filter.Statuses);
        }

        void OnDataSourceChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(Reload));
            }
            else
            {
                Reload();
            }
        }

        public void ReloadWithNetworkCheck()
        {
            if (Network.NetworkStatus != Util.ConnectionOKStatus)
            {
                reconnect_LinkClicked(null, null);
            }

            Reload();
        }

        public void Reload()
        {
            this.connectionError.Visible = Network.NetworkStatus != Util.ConnectionOKStatus;
            this.errorMessage.Text = Network.NetworkMessage;
            if (Network.NetworkStatus != Util.ConnectionOKStatus) {
                return; 
            }

            var reload =
                DnsServiceManager.GetRequestList("filtered requests lookup", Network, Page * _pageSize, _pageSize, Network.Filter, _sortColumn, _isSortAscending)
                .TakeLast(1)
                .ObserveOn(this)
                .Select(list =>
                {
                    PageCount = list.TotalCount / _pageSize + (list.TotalCount == 0 || list.TotalCount % _pageSize > 0 ? 1 : 0);
                    lblTotalPages.Text = PageCount.ToString();
                    if (Page > PageCount)
                    {
                        Page = PageCount;
                        txtPageNumber.Text = (Page + 1).ToString();
                    }

                    var selected = SelectedRequest;
                    dgvRequestList.DataSource = list.Segment;
                    SelectedRequest = selected == null ? null : list.Segment.FirstOrDefault(rq => rq.ID == selected.ID && rq.DataMartID == selected.DataMartID);

                    DisplaySortGlyph();
                    EnableDisablePageButtons();

                    return Unit.Default;
                });

            var now = DateTime.Now;

            var newReqsLookup =
                DnsServiceManager.GetRequestList("new requests lookup", Network, 0, 1, new RequestFilter { DateRange = DateRangeKind.Exact, FromDate = _lastNewRequestsLookup }, null, null)
                .Take(1)
                .ObserveOn(this)
                .Select(n =>
                {
                    if (n.TotalCount > 0) NewRequestsAvailable = true;
                    _lastNewRequestsLookup = now.AddMilliseconds(1);
                    return Unit.Default;
                });

            var invFilter = InvertFilter(_lastInvisibleRequestsLookup);
            var invisibleReqsLookup = invFilter == null ? Observable.Return(Unit.Default) :
                DnsServiceManager.GetRequestList("hidden requests lookup", Network, 0, 1, invFilter.Value, null, null)
                .Take(1)
                .ObserveOn(this)
                .Select(n =>
                {
                    if (n.TotalCount > 0) InvisibleRequestsAvailable = true;
                    _lastInvisibleRequestsLookup = now.AddMilliseconds(1);
                    return Unit.Default;
                });

            _currentReload.CallDispose();

            _currentReload = Observable
                .Merge(reload, newReqsLookup, invisibleReqsLookup)
                .TakeLast(1)
                .Do(_ =>
                {
                    _currentReload = null;
                    Reloaded.Raise(this);
                })
                .Finally(() => _currentReload = null)
                .Catch((Exception ex) =>
                {
                    _log.Error(ex);
                    Network.NetworkStatus = Util.ConnectionFailedStatus;
                    errorMessage.Text = Network.NetworkMessage = ex.Message;
                    connectionError.Show();
                    _currentReload = null;
                    ReloadFailed.Raise(this);
                    return Observable.Empty<Unit>();
                })
                .Subscribe();

            ReloadStarted.Raise(this);
        }

        private RequestFilter? InvertFilter(DateTime fromDate)
        {
            var f = Filter;
            if ((f.DataMartIds.NullOrEmpty() || f.DataMartIds.Length == Network.DataMartList.Count)
                &&
                (f.Statuses.NullOrEmpty() || f.Statuses.Length == HubRequestStatus.All.Count))
            {
                return null;
            }

            return new RequestFilter
            {
                DataMartIds = Network.DataMartList.EmptyIfNull().Select(d => d.DataMartId).Except(f.DataMartIds.EmptyIfNull()).ToArray(),
                Statuses = HubRequestStatus.All.Where(s => f.Statuses.EmptyIfNull().Contains(s.Code) == false).Select(s => s.Code).ToArray(),
                DateRange = DateRangeKind.Exact,
                FromDate = fromDate
            };
        }

        private void dgvRequestList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var newColumn = (RequestSortColumn)dgvRequestList.Columns[e.ColumnIndex].Tag;
            _isSortAscending = (_sortColumn == newColumn && _isSortAscending != null) ? !_isSortAscending : true;
            _sortColumn = newColumn;
            SortModeChanged.Raise(this);
            Reload();
        }

        private void DisplaySortGlyph()
        {
            foreach (DataGridViewColumn c in dgvRequestList.Columns)
            {
                c.HeaderCell.SortGlyphDirection =
                    ((RequestSortColumn)c.Tag == _sortColumn && _isSortAscending != null)
                    ? _isSortAscending.Value ? SortOrder.Ascending : SortOrder.Descending
                    : SortOrder.None;
            }
        }

        private void btn_First_Click(object sender, EventArgs e)
        {
            Page = 0;
            txtPageNumber.Text = (Page + 1).ToString();
            Reload();
        }

        private void btn_Last_Click(object sender, EventArgs e)
        {
            Page = PageCount - 1;
            txtPageNumber.Text = (Page + 1).ToString();
            Reload();
        }

        private void btn_Previous_Click(object sender, EventArgs e)
        {
            if (Page > 0)
            {
                Page--;
                txtPageNumber.Text = (Page + 1).ToString();
                Reload();
            }
        }

        private void btn_Next_Click(object sender, EventArgs e)
        {
            if (Page < PageCount - 1)
            {
                Page++;
                txtPageNumber.Text = (Page + 1).ToString();
                Reload();
            }
        }

        private void txtPageNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!(Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar)))
                    e.Handled = true;

                if (e.KeyChar == (char)Keys.Return && txtPageNumber.Text.All(char.IsDigit))
                {
                    Page = int.Parse(txtPageNumber.Text) - 1;
                    Reload();
                }
            }
            catch (Exception)
            {
            }
        }

        private void EnableDisablePageButtons()
        {
            btn_First.Enabled = btn_Previous.Enabled = Page > 0;
            btn_Last.Enabled = btn_Next.Enabled = Page < PageCount - 1;
        }

        private void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            int p;
            if (!int.TryParse(cmbPageSize.Text, out p)) return;

            PageSize = p;
            PageSizeChanged.Raise(this);
        }

        private void cmbPageSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar))) e.Handled = true;
            if (e.KeyChar == (char)Keys.Return)
            {
                cmbPageSize_SelectedIndexChanged(null, null);
                e.Handled = true;
            }
        }

        private void dgvRequestList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var r = SelectedRequest;
            if (r != null)
            {
                RequestRowDoubleClick.Raise(sender, new RequestDetailEventArgs { Network = Network, RequestID = r.ID, DataMartID = r.DataMartID });
            }
        }

        private void cmbStatus_SelectionChanged(object sender, EventArgs e)
        {
            ChangeFilter(f => f.Statuses = cmbStatus.GetSelected<Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus>().EmptyIfNull().ToArray());
            SetStatusText();
        }

        private void RemoveDefinedCustomDateFilter()
        {
            for (int i = cmbDates.Items.Count - 1; i >= 0; i--)
            {
                var item = cmbDates.Items[i] as DateFilterItem;
                if (item != null && item.IsDefinedCustom) cmbDates.Items.RemoveAt(i);
            }
        }

        private void cmbDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = cmbDates.SelectedItem as DateFilterItem;
            if (item == null) return;

            if (item.IsOpenCustom)
            {
                var f = new CustomDateFilterForm(Filter);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    RemoveDefinedCustomDateFilter();
                    Filter = f.Filter;
                }
                SetDateFilterSelectedItem(Filter);
            }
            else if (item.IsDaysBack)
            {
                RemoveDefinedCustomDateFilter();
                ChangeFilter(f =>
                {
                    f.DateRange = DateRangeKind.RecentDays;
                    f.RecentDaysToShow = item.DaysBack;
                });
            }
            else if (item.IsDefinedCustom)
            {
                ChangeFilter(f =>
                {
                    f.DateRange = DateRangeKind.Exact;
                    f.FromDate = item.CustomFrom;
                    f.ToDate = item.CustomTo;
                });
            }

            btn_First_Click(sender, e);
        }

        private void cmbDataMarts_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dataMartDescription = cmbDataMarts.SelectedValue as DataMartDescription;
            if (dataMartDescription != null)
            {
                ChangeFilter(f => f.DataMartIds = dataMartDescription.DataMartId == Configuration.ALL_DATAMARTS_ID ? null : new[] { dataMartDescription.DataMartId });
            }
        }

        private void reconnect_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var progress = new ProgressForm("Connecting to the Network", "Connecting to the Network");
            Observable
                .Start(() => DnsServiceManager.TestConnections(new[] { Network }), Scheduler.Default)
                .TakeUntil(progress.ShowAndWaitForCancel(this))
                .ObserveOn(this)
                .Finally(progress.Dispose)
                .Do(_ => Reload())
                .LogExceptions(_log.Error)
                .Catch()
                .Subscribe();
        }

        private void resetFilters_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Filter = new RequestFilter();
            invisibleRequestsWarning.Hide();
        }

        private void hideInvisibleRequestsWarning_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            invisibleRequestsWarning.Hide();
        }

        private void dgvRequestList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //format the routing status column
            if (e.ColumnIndex == 7)
            {
                if (e.DesiredType != typeof(string))
                    return;

                try
                {
                    e.Value = HubRequestStatus.GetDescription((DTO.DataMartClient.Enums.DMCRoutingStatus)e.Value);
                }
                catch
                {
                    e.Value = "Unable to Determine: " + e.Value;
                }
            }
        }

        private void dgvRequestList_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) && SelectedRequest != null)
            {   
                e.SuppressKeyPress = true;
                e.Handled = true;
                RequestRowDoubleClick.Raise(sender, new RequestDetailEventArgs { Network = Network, RequestID = SelectedRequest.ID, DataMartID = SelectedRequest.DataMartID });
            }
        }

        private void dgvRequestList_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) && SelectedRequest != null)
            {
                //if you don't suppress the keydown event the grid will automatically move to the next row before the keyup event is handled.
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }

    public class RequestDetailEventArgs : EventArgs
    {
        public NetWorkSetting Network { get; set; }
        public Guid RequestID { get; set; }
        public Guid DataMartID { get; set; }
    }

    class DateFilterItem
    {
        public string Name { get; set; }
        public bool IsDaysBack { get; set; }
        public int DaysBack { get; set; }
        public bool IsOpenCustom { get; set; }

        public bool IsDefinedCustom { get { return !IsOpenCustom && !IsDaysBack; } }
        public DateTime CustomFrom { get; set; }
        public DateTime CustomTo { get; set; }

        public DateFilterItem(string name, int daysBack)
        {
            Name = name;
            DaysBack = daysBack;
            IsDaysBack = true;
        }

        public DateFilterItem(string name)
        {
            Name = name; IsOpenCustom = true;
        }

        public DateFilterItem(DateTime customFrom, DateTime customTo)
        {
            Name = string.Format("Custom {0:MM/dd/yyyy} - {1:MM/dd/yyyy}", customFrom, customTo);
            CustomTo = customTo;
            CustomFrom = customFrom;
        }

        public override string ToString() { return Name; }
    }
}