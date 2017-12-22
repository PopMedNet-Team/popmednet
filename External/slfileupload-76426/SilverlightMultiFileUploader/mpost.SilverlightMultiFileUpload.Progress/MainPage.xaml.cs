using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using mpost.SilverlightMultiFileUpload.Progress.Classes;
using System.Windows.Messaging;

/*
* Copyright Michiel Post
* http://www.michielpost.nl
* contact@michielpost.nl
*
* http://www.codeplex.com/SLFileUpload/
*
* */

namespace mpost.SilverlightMultiFileUpload.Progress
{
    public partial class MainPage : UserControl
    {
        private LocalMessageReceiver _receiver;
        private UploadInfo _uploadInfo;

        public MainPage()
        {
            InitializeComponent();

            _uploadInfo = new UploadInfo();
            this.DataContext = _uploadInfo;

            //Subscribe to update messages
            _receiver = new LocalMessageReceiver("SLMFU");
            _receiver.MessageReceived += new EventHandler<MessageReceivedEventArgs>(_receiver_MessageReceived);
            _receiver.Listen();

            
        }

        /// <summary>
        /// Message received from other Silverlight control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _receiver_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                float percentage = float.Parse(e.Message);
                _uploadInfo.Percentage = percentage;

                //Show smooth progress animation
                if (percentage < TotalProgress.Value)
                    TotalProgress.Value = percentage;
                else
                {
                    sbProgressFrame.Value = percentage;
                    sbProgress.Begin();
                }
            }
            catch { }
        }
       
    }
}
