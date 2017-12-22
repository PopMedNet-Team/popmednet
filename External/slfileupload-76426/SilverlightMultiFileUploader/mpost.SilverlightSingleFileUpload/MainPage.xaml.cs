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
using mpost.SilverlightMultiFileUpload.Utils.Constants;
using System.IO;
using mpost.SilverlightMultiFileUpload.Contracts;
using System.Windows.Markup;
using mpost.SilverlightMultiFileUpload.Controls;

namespace mpost.SilverlightSingleFileUpload
{
  public partial class MainPage : UserControl
  {
     private FileCollection _files;
     private IVisualizeFileRow _fileRow;

     public MainPage()
        {
            InitializeComponent();

            SetRowTemplate(new FileRowControl());

            _files = new FileCollection(Configuration.Instance.CustomParams, Configuration.Instance.MaxUploads, this.Dispatcher);

            HtmlPage.RegisterScriptableObject("Files", _files);
            HtmlPage.RegisterScriptableObject("Control", this);
            HtmlPage.RegisterScriptableObject("Configuration", Configuration.Instance);

           
            this.Loaded += new RoutedEventHandler(Page_Loaded);
            _files.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_files_CollectionChanged);
            _files.AllFilesFinished += new EventHandler(_files_AllFilesFinished);
           
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
            if (_files.TotalUploadedFiles == 0)
            {
                if (_files.Count == 0)
                {
                    VisualStateManager.GoToState(this, "Empty", true);
                    SelectFilesButton.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {

                    if (_files.FirstOrDefault(f => f.State == Enums.FileStates.Uploading) != null)
                        VisualStateManager.GoToState(this, "Uploading", true);
                    else if (_files.FirstOrDefault(f => f.State == Enums.FileStates.Finished) != null)
                        VisualStateManager.GoToState(this, "Finished", true);
                    else
                    {
                        SelectFilesButton.Visibility = System.Windows.Visibility.Collapsed;
                        VisualStateManager.GoToState(this, "Selected", true);
                    }
                }
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

        /// <summary>
        /// Open the select file dialog
        /// </summary>
        private void SelectUserFiles()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;

            try
            {
                //Check the file filter (filter is used to filter file extensions to select, for example only .jpg files)
                if (!string.IsNullOrEmpty(Configuration.Instance.FileFilter))
                    ofd.Filter = Configuration.Instance.FileFilter;
            }
            catch (ArgumentException ex)
            {
                //User supplied a wrong configuration file
                //throw new Exception(UserMessages.ErrorFileFilterConfig, ex);
            }

            if (ofd.ShowDialog() == true)
            {
                foreach (FileInfo file in ofd.Files)
                {
                    AddFile(file);
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

                    ((IVisualizeFileRow)_fileRow).UserFile = userFile;
                    ((FrameworkElement)_fileRow).Visibility = System.Windows.Visibility.Visible;

                }
                else
                {

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
        public void SetRowTemplate(IVisualizeFileRow type)
        {
            FrameworkElement fileRowControl = (FrameworkElement)type;
          _fileRow = type;
          fileRowControl.Visibility = System.Windows.Visibility.Collapsed;
         

          FileList.Children.Add(fileRowControl);
            //FileList.Children.Add(DataTemplateHelper.CreateDataTemplate(type));
        }
       

        #endregion

    }
}
