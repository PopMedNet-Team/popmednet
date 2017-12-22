using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lpp;

namespace Lpp.Dns.DataMart.Client.Controls
{
    public partial class CheckBoxDropDownWpf : UserControl
    {
        private IList<CheckedItem> _items;

        public void SetList<T>( IEnumerable<T> allItems, IEnumerable<T> selectedItems, Func<T,string> display )
        {
            _items = (from i in allItems.EmptyIfNull()
                      select new CheckedItem
                      {
                          Item = i,
                          Display = display(i),
                          Checked = selectedItems.EmptyIfNull().Contains(i),
                          Owner = this
                      }).ToList();

            this.list.ItemsSource = _items;
        }

        public IEnumerable<T> GetSelected<T>()
        {
            return _items.Where( i => i.Checked ).Select( i => i.Item == null ? default( T ) : (T) i.Item );
        }

        public event EventHandler SelectionChanged;

        public CheckBoxDropDownWpf()
        {
            InitializeComponent();
        }

        private void Button_Click_1( object sender, RoutedEventArgs e )
        {
            p.IsOpen = !p.IsOpen;
        }

        public void OnChanged()
        {
            SelectionChanged.Raise( this );
        }
    }

    public class CheckedItem
    {
        private bool _checked;
        public bool Checked 
        { 
            get 
            {
                return _checked; 
            }
            set
            {
                _checked = value;
                if ( Owner != null ) 
                {
                    Owner.OnChanged(); 
                }
            }
        }
        public object Item { get; set; }
        public string Display { get; set; }
        public CheckBoxDropDownWpf Owner { get; set; }
    }

    public class CheckBoxDropDown : System.Windows.Forms.Control
    {
        private readonly CheckBoxDropDownWpf _child = new CheckBoxDropDownWpf();

        public event EventHandler SelectionChanged
        {
            add { _child.SelectionChanged += value; }
            remove { _child.SelectionChanged -= value; }
        }

        public CheckBoxDropDown()
	    {
            this.Controls.Add( new System.Windows.Forms.Integration.ElementHost { Child = _child, Dock = System.Windows.Forms.DockStyle.Fill } );
	    }

        public new string Text 
        {
            get
            {
                return _child.text.Text;
            }
            set 
            {
                _child.text.Text = value; 
            }
        }

        public void SetList<T>( IEnumerable<T> allItems, IEnumerable<T> selectedItems, Func<T,string> display )
        {
            _child.SetList( allItems, selectedItems, display );
        }

        public IEnumerable<T> GetSelected<T>() { return _child.GetSelected<T>(); }

        protected override void OnEnabledChanged( EventArgs e )
        {
            base.OnEnabledChanged( e );
            _child.IsEnabled = this.Enabled;
        }
    }
}