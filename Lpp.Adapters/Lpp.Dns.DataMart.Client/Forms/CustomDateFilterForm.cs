using System;
using System.Windows.Forms;
using Lpp.Dns.DataMart.Lib;

namespace Lpp.Dns.DataMart.Client
{
    public partial class CustomDateFilterForm : Form
    {
        private RequestFilter _filter;
        public RequestFilter Filter
        {
            get
            {
                _filter.FromDate = Maybe.Parse<DateTime>( DateTime.TryParse, dtpFrom.Text ).Select(d => d.ToUniversalTime()).AsNullable();
                _filter.ToDate = Maybe.Parse<DateTime>( DateTime.TryParse, dtpTo.Text ).Select( d => d.ToUniversalTime().AddDays(1).AddSeconds(-1) ).AsNullable();
                _filter.DateRange = DateRangeKind.Exact;
                return _filter;
            }
            set
            {
                _filter = value;
                dtpFrom.Value = _filter.EffectiveFromDate.ToLocalTime();
                dtpTo.Value = _filter.EffectiveToDate.ToLocalTime() >= dtpTo.MinDate ? _filter.EffectiveToDate.ToLocalTime() : dtpTo.MinDate;
            }
        }

        public CustomDateFilterForm( RequestFilter filter )
        {
            InitializeComponent();
            Filter = filter;
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            dtpTo.MinDate = dtpFrom.Value;
        }
    }
}