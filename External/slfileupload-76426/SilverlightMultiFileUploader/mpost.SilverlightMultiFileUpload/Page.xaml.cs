using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using mpost.SilverlightMultiFileUpload.Classes;
using System.IO;
using System.Windows.Browser;
using mpost.SilverlightMultiFileUpload.Core;
using mpost.SilverlightMultiFileUpload.Contracts;
using System.Windows.Markup;
using mpost.SilverlightMultiFileUpload.Utils.Constants;
using mpost.SilverlightMultiFileUpload.Controls;
using mpost.SilverlightMultiFileUpload.Utils.Helpers;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace mpost.SilverlightMultiFileUpload
{
    [ScriptableType]
    public partial class Page : UserControl
    {
        private FileCollection _files;

        public Page()
        {
            InitializeComponent();

            SetRowTemplate(typeof(FileRowControl));

            _files = new FileCollection(Configuration.Instance.CustomParams, Configuration.Instance.MaxUploads, this.Dispatcher);

            HtmlPage.RegisterScriptableObject("Files", _files);
            HtmlPage.RegisterScriptableObject("Control", this);
            HtmlPage.RegisterScriptableObject("Configuration", Configuration.Instance);

            FileList.ItemsSource = _files;
            FilesCount.DataContext = _files;
            TotalProgress.DataContext = _files;
            PercentLabel.DataContext = _files;
            TotalKB.DataContext = _files;

            this.Loaded += new RoutedEventHandler(Page_Loaded);
            _files.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_files_CollectionChanged);
            _files.AllFilesFinished += new EventHandler(_files_AllFilesFinished);
            _files.TotalPercentageChanged += new EventHandler(_files_TotalPercentageChanged);
        }

        void _files_TotalPercentageChanged(object sender, EventArgs e)
        {
            // if the percentage is decreasing, don't use an animation
            if (_files.Percentage < TotalProgress.Value)
                TotalProgress.Value = _files.Percentage;
            else
            {
                sbProgressFrame.Value = _files.Percentage;
                sbProgress.Begin();
            }
        }

        void _files_AllFilesFinished(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(this, "Finished", true);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Empty", false);
        }

        void _files_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (_files.Count == 0)
            {
                VisualStateManager.GoToState(this, "Empty", true);
            }
            else
            {

                if (_files.FirstOrDefault(f => f.State == Enums.FileStates.Uploading) != null)
                    VisualStateManager.GoToState(this, "Uploading", true);
                else if (_files.FirstOrDefault(f => f.State == Enums.FileStates.Finished) != null)
                    VisualStateManager.GoToState(this, "Finished", true);
                else
                    VisualStateManager.GoToState(this, "Selected", true);
            }
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
                        MessageChildWindow messageWindow = new MessageChildWindow();
                        messageWindow.Message = string.Format(UserMessages.AddFileError, file.Name);
                        messageWindow.Show();
                    }
                }
            }
        }





        /// <summary>
        /// Upload Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            UploadFiles();
        }

        /// <summary>
        /// Start uploading files
        /// </summary>
        private void UploadFiles()
        {
            if (_files.Count == 0)
            {
                MessageChildWindow messageWindow = new MessageChildWindow();
                messageWindow.Message = UserMessages.ErrorNoFilesSelected;
                messageWindow.Show();
            }
            else
            {
                //Tell the collection to start uploading
                _files.UploadFiles();
            }
        }

        /// <summary>
        /// Clear button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFilesList();
        }

        /// <summary>
        /// Clear the file list
        /// </summary>
        private void ClearFilesList()
        {
            _files.Clear();

        }

        /// <summary>
        /// Adds file to file collection
        /// </summary>
        /// <param name="file"></param>
        private void AddFile(FileInfo file)
        {
            if (file.Exists)
            {
                string fileName = file.Name;

                //Create a new UserFile object
                UserFile userFile = new UserFile();
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
                    MessageChildWindow messageWindow = new MessageChildWindow();
                    messageWindow.Message = UserMessages.MaxFileSize + (Configuration.Instance.MaxFileSize / 1024).ToString() + "KB.";
                    messageWindow.Show();

                    if (MaximumFileSizeReached != null)
                        MaximumFileSizeReached(this, null);

                }
            }
        }

        #region Plugin Helpers

        /// <summary>
        /// Sets a usercontrol as DataTemplate for the list of files
        /// </summary>
        /// <param name="type"></param>
        public void SetRowTemplate(Type type)
        {
            FileList.ItemTemplate = DataTemplateHelper.CreateDataTemplate(type);
        }
       

        #endregion

    }
}
