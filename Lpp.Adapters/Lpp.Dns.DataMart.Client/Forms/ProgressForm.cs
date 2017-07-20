using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Concurrency;

namespace Lpp.Dns.DataMart.Client
{
    public partial class ProgressForm : Form
    {
        public bool Indeteminate { get { return progress.Style == ProgressBarStyle.Marquee; } set { progress.Style = value ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks; } }
        public int Progress { get { return progress.Value; } set { progress.Value = Math.Max( progress.Minimum, Math.Min( value, progress.Maximum ) ); } }

        public IObservable<DialogResult> ShowAndWaitForCancel( Control owner )
        {
            return Observable.Defer( () => Observable.Start( () => this.ShowDialog( owner ), new ControlScheduler( owner ) ) );
        }

        public ProgressForm( string title, string text ) : this()
        {
            this.Text = title;
            this.text.Text = text;
        }

        private ProgressForm()
        {
            InitializeComponent();
        }
    }
}