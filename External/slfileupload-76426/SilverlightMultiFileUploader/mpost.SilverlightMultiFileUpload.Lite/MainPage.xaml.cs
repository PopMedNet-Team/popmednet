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
using mpost.SilverlightMultiFileUpload.Core;
using System.Windows.Browser;
using System.IO;
using System.Windows.Messaging;
using mpost.SilverlightMultiFileUpload.Contracts;

/*
* Copyright Michiel Post
* http://www.michielpost.nl
* contact@michielpost.nl
*
* http://www.codeplex.com/SLFileUpload/
*
* */

namespace mpost.SilverlightMultiFileUpload.Lite
{
    public partial class MainPage : UserControl
    {
        private FileCollection _files;
        private LocalMessageSender _localSender;
        
        public MainPage()
        {            
            InitializeComponent();

            _localSender = new LocalMessageSender("SLMFU");
           

            _files = new FileCollection(Configuration.Instance.CustomParams, Configuration.Instance.MaxUploads, this.Dispatcher);
            _files.TotalPercentageChanged += new EventHandler(_files_TotalPercentageChanged);

            HtmlPage.RegisterScriptableObject("Files", _files);
            HtmlPage.RegisterScriptableObject("Control", this);
            HtmlPage.RegisterScriptableObject("Configuration", Configuration.Instance);
         

        }

        void _files_TotalPercentageChanged(object sender, EventArgs e)
        {
            _localSender.SendAsync(_files.Percentage.ToString());
        }

      

        ///////////////////////////////////////////////////////////
        //Scriptable members to control functions via javascript

        [ScriptableMember]
        public void StartUpload()
        {
            UploadFiles();
        }

        [ScriptableMember]
        public void ClearList()
        {
            ClearFilesList();
        }

        [ScriptableMember]
        public void RemoveAt(int index)
        {
            if (_files.Count > index)
                _files.RemoveAt(index);
        }  

        [ScriptableMember()]
        public event EventHandler MaximumFileSizeReached;

        ///////////////////////////////////////////////////////////
     


        /// <summary>
        /// Select files button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFilesButton_Click(object sender, RoutedEventArgs e)
        {
            SelectUserFiles();
        }

        /// <summary>
        /// Drag and drop of files is supported
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_Drop(object sender, DragEventArgs e)
        {
            FileInfo[] files = (FileInfo[])e.Data.GetData(System.Windows.DataFormats.FileDrop);  
           
            foreach (FileInfo file in files)
            {
                AddFile(file);
            }
            
        }

        /// <summary>
        /// Open the select file dialog
        /// </summary>
        private void SelectUserFiles()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;

            try
            {
                //Check the file filter (filter is used to filter file extensions to select, for example only .jpg files)
                if (!string.IsNullOrEmpty(Configuration.Instance.FileFilter))
                    ofd.Filter = Configuration.Instance.FileFilter;
            }
            catch (ArgumentException ex)
            {
                //User supplied a wrong configuration file
                throw new Exception(UserMessages.ErrorFileFilterConfig, ex);
            }

            if (ofd.ShowDialog() == true)
            {
                foreach (FileInfo file in ofd.Files)
                {
                    try
                    {

                        AddFile(file);
                    }
                    catch
                    {
                        //Unable to add file
                    }
                }
            }
        }
        
       
        /// <summary>
        /// Start uploading files
        /// </summary>
        private void UploadFiles()
        {
            if (_files.Count > 0)
            {
                //Tell the collection to start uploading
                _files.UploadFiles();
            }            
        }       
       

        /// <summary>
        /// Clear the file list
        /// </summary>
        private void ClearFilesList()
        {
            _files.Clear();
        }

        private void AddFile(FileInfo file)
        {
            if (file.Exists)
            {
                string fileName = file.Name;

                //Create a new UserFile object
                IUserFile userFile = new UserFile();
                userFile.FileName = file.Name;
                userFile.FileStream = file.OpenRead();


                //Check for the file size limit (configurable)
                if (userFile.FileStream.Length <= Configuration.Instance.MaxFileSize)
                {
                    //Add to the list
                    _files.Add(userFile);
                }
                else
                {
                    //MessageChildWindow messageWindow = new MessageChildWindow();
                    //messageWindow.Message = UserMessages.MaxFileSize + (_maxFileSize / 1024).ToString() + "KB.";
                    //messageWindow.Show();

                    if (MaximumFileSizeReached != null)
                        MaximumFileSizeReached(this, null);

                }
            }
        }
    }
}
