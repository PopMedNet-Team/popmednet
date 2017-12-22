using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Browser;
using System.Collections.Generic;
using System.Windows.Threading;
using mpost.SilverlightMultiFileUpload.Contracts;
using mpost.SilverlightMultiFileUpload.Utils.Constants;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace mpost.SilverlightMultiFileUpload.Core
{
    [ScriptableType]
    public class FileCollection : ObservableCollection<IUserFile>
    {
        private double _bytesUploaded = 0;
        private float _percentage = 0;
        private string _customParams;
        private int _maxUpload;
        private int _totalUploadedFiles = 0;

        private Dispatcher _uiDispatcher;

        public int CurrentUploads
        {
            get
            {
                int count = 0;
                foreach (IUserFile file in this)
                {
                    if (file.State == Enums.FileStates.Uploading)
                        count++;
                }

                return count;
            }
        }

        public double BytesUploaded
        {
            get { return _bytesUploaded; }
            set
            {
                _bytesUploaded = value;

                this.OnPropertyChanged(new PropertyChangedEventArgs("BytesUploaded"));
            }
        }

        [ScriptableMember()]
        public string CustomParams
        {
            get { return _customParams; }
            set
            {
                _customParams = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("CustomParams"));
            }
        }


        [ScriptableMember()]
        public int TotalFilesSelected
        {
            get { return this.Items.Count; }           
        }

        [ScriptableMember()]
        public float Percentage
        {
            get { return _percentage; }
            set
            {
                _percentage = value;

                this.OnPropertyChanged(new PropertyChangedEventArgs("Percentage"));   
            
                //Notify Javascript of percentage change
                if (TotalPercentageChanged != null)
                    TotalPercentageChanged(this, null);
            }
        }

        [ScriptableMember()]
        public int TotalUploadedFiles
        {
            get { return _totalUploadedFiles; }
            set
            {
                _totalUploadedFiles = value;

                this.OnPropertyChanged(new PropertyChangedEventArgs("TotalUploadedFiles"));
            }
        }

        [ScriptableMember()]
        public IList<IUserFile> FileList
        {
            get { return this.Items; }
        }

        public bool IsUploading { get; set; }

        [ScriptableMember()]
        public event EventHandler SingleFileUploadFinished;

        [ScriptableMember()]
        public event EventHandler AllFilesFinished;

        [ScriptableMember()]
        public event EventHandler ErrorOccurred;

        [ScriptableMember()]
        public event EventHandler FileAdded;

        [ScriptableMember()]
        public event EventHandler FileRemoved;

        [ScriptableMember()]
        public event EventHandler StateChanged;

        [ScriptableMember()]
        public event EventHandler TotalPercentageChanged;

        /// <summary>
        /// FileCollection constructor
        /// </summary>
        /// <param name="customParams"></param>
        /// <param name="maxUploads"></param>
        public FileCollection(string customParams, int maxUploads, Dispatcher uiDispatcher)
        {
            _customParams = customParams;
            _maxUpload = maxUploads;
            _uiDispatcher = uiDispatcher;

            this.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(FileCollection_CollectionChanged);
            
        }

       
        /// <summary>
        /// Add a new file to the file collection
        /// </summary>
        /// <param name="item"></param>
        public new void Add(IUserFile item)
        {
            //Listen to the property changed for each added item
            item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);

            base.Add(item);

            if (FileAdded != null)
                FileAdded(this, null);
        }

        /// <summary>
        /// Removed an existing user file to the collection
        /// </summary>
        /// <param name="item"></param>
        public new void Remove(IUserFile item)
        {
            base.Remove(item);

            if (FileRemoved != null)
                FileRemoved(this, null);
        }

        /// <summary>
        /// Clears the complete list
        /// </summary>
        public new void Clear()
        {
            foreach (UserFile file in this)
                file.CancelUpload();

            base.Clear();

            IsUploading = false;

            if (FileRemoved != null)
                FileRemoved(this, null);
        }

        /// <summary>
        /// Removes an item specified by the index
        /// </summary>
        /// <param name="index"></param>
        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);

            if (FileRemoved != null)
                FileRemoved(this, null);
        }

        /// <summary>
        /// Start uploading files
        /// </summary>
        public void UploadFiles()
        {
            lock (this)
            {
                foreach (IUserFile file in this)
                {
                    if (file.State == Enums.FileStates.Pending
                        && CurrentUploads < _maxUpload)
                    {
                        IsUploading = true;

                        file.Upload(_customParams, _uiDispatcher);                        
                    }
                }
            }

        }

        /// <summary>
        /// Recount statistics
        /// </summary>
        private void RecountTotal()
        {
            //Recount total
            double totalSize = 0;
            double totalSizeDone = 0;

            foreach (IUserFile file in this)
            {
                //totalSize += file.FileSize;
                //totalSizeDone += file.BytesUploaded;

                totalSize += file.FileSize;
                if (file.State == Enums.FileStates.Error)
                {
                    totalSizeDone += file.FileSize; //If error occures, the whole file is done
                }
                else
                {
                    totalSizeDone += file.BytesUploadedFinished;
                }
            }

            double percentage = 0;

            if (totalSize > 0)
                percentage = totalSizeDone / totalSize;

            BytesUploaded = totalSizeDone;

            Percentage = (float)percentage;
        }

        /// <summary>
        /// Check if all files are finished uploading
        /// </summary>
        private void AreAllFilesFinished()
        {
            if (Percentage == 1f)
            {
                IsUploading = false;

                if (AllFilesFinished != null)
                    AllFilesFinished(this, null);
            }
        }

        /// <summary>
        /// The collection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //Recount total when the collection changed (items added or deleted)
            RecountTotal();
        }

        /// <summary>
        /// Property of individual item changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State")
            {
                IUserFile file = (UserFile)sender;
                if (file.State == Enums.FileStates.Finished)
                {                   
                    TotalUploadedFiles++;

                    UploadFiles();

                    if (SingleFileUploadFinished != null)
                        SingleFileUploadFinished(this, new FileUploadedEventArgs(file.FileName, file.FileSize));                    
                   
                }
                else if (file.State == Enums.FileStates.Error)
                {       
                    UploadFiles();

                    if (ErrorOccurred != null)
                        ErrorOccurred(this, null);
                }
                else if (file.State == Enums.FileStates.Deleted)
                {                    
                    this.Remove(file);

                    file = null;

                    if(IsUploading)
                        UploadFiles();

                }

                if (StateChanged != null)
                    StateChanged(this, null);

                RecountTotal();

                AreAllFilesFinished();


            }
            else if (e.PropertyName == "BytesUploaded" || e.PropertyName == "BytesUploadedFinished")
            {
                RecountTotal();
            }
        }
     
    }
}
