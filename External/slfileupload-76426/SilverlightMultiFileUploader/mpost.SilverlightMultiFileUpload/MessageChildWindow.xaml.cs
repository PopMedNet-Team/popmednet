using System.Windows;
using System.Windows.Controls;

/*
* Copyright Michiel Post
* http://www.michielpost.nl
* contact@michielpost.nl
*
* http://www.codeplex.com/SLFileUpload/
*
* */

namespace mpost.SilverlightMultiFileUpload
{
    public partial class MessageChildWindow : ChildWindow
    {
        public string Message { get; set; }

        public MessageChildWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }      
    }
}

