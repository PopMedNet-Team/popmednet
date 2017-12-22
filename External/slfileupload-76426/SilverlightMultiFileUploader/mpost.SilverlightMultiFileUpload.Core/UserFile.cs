using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Threading;
using System.Windows.Browser;
using mpost.SilverlightMultiFileUpload.Contracts;
using mpost.SilverlightMultiFileUpload.Utils.Constants;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace mpost.SilverlightMultiFileUpload.Core
{
    public class UserFile : INotifyPropertyChanged, IUserFile
    {
        private string _fileName;             
        private Stream _fileStream;
        private Enums.FileStates _state = Enums.FileStates.Pending;
        private double _bytesUploaded = 0;
        private double _bytesUploadedFinished = 0;
        private double _fileSize = 0;
        private float _percentage = 0;
        private float _percentageFinished = 0;
        private IFileUploader _fileUploader;
        
        [ScriptableMember()]
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        public Enums.FileStates State
        {
            get { return _state; }
            set
            {
                _state = value;


                NotifyPropertyChanged("State");
            }
        }

        [ScriptableMember()]
        public string StateString
        {
            get { return _state.ToString(); }
           
        }

        public Stream FileStream
        {
            get { return _fileStream; }
            set
            {
                _fileStream = value;

                if (_fileStream != null)
                    _fileSize = _fileStream.Length;
                
                
            }
        }

        [ScriptableMember()]
        public double FileSize
        {
            get {
                return _fileSize;               
            }
        }

        public double BytesUploaded
        {
            get { return _bytesUploaded; }
            set
            {
                _bytesUploaded = value;

                NotifyPropertyChanged("BytesUploaded");

                Percentage = (float)(value / FileSize);

            }
        }

        [ScriptableMember()]
        public double BytesUploadedFinished
        {
            get { return _bytesUploadedFinished; }
            set
            {
                _bytesUploadedFinished = value;

                NotifyPropertyChanged("BytesUploadedFinished");

                PercentageFinished = (float)(value / FileSize);

            }
        }

        /// <summary>
        /// From 0 to 1
        /// </summary>
        [ScriptableMember()]
        public float Percentage
        {
            get { return _percentage; }
            set
            {
                _percentage = value;
                NotifyPropertyChanged("Percentage");


            }
        }

        /// <summary>
        /// From 0 to 1
        /// </summary>
        [ScriptableMember()]
        public float PercentageFinished
        {
            get { return _percentageFinished; }
            set
            {
                _percentageFinished = value;
                NotifyPropertyChanged("PercentageFinished");


            }
        }


        public string ErrorMessage { get; set; }
      

        public void Upload(string initParams, Dispatcher uiDispatcher)
        {
            this.State = Enums.FileStates.Uploading;

            _fileUploader = new HttpFileUploader(this, uiDispatcher);
            
            _fileUploader.StartUpload(initParams);
            _fileUploader.UploadFinished += new EventHandler(fileUploader_UploadFinished);          
            
        }

        public void CancelUpload()
        {
            if (this.FileStream != null)
            {
                this.FileStream.Close();
                this.FileStream.Dispose();
                this.FileStream = null;
            }

            if (_fileUploader != null && this.State == Enums.FileStates.Uploading)
            {
                _fileUploader.CancelUpload();
            }

            _fileUploader = null;
        }

        private void fileUploader_UploadFinished(object sender, EventArgs e)
        {
            _fileUploader = null;

            if (this.State != Enums.FileStates.Deleted
               && this.State != Enums.FileStates.Error)
            {
                this.State = Enums.FileStates.Finished;

                if (this.FileStream != null)
                {
                    this.FileStream.Close();
                    this.FileStream.Dispose();
                    this.FileStream = null;
                }
            }
        }

        #region INotifyPropertyChanged Members

        private void NotifyPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
