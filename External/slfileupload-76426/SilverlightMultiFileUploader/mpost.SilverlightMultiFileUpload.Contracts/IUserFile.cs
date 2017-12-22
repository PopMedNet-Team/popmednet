using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Threading;
using System.IO;
using mpost.SilverlightMultiFileUpload.Utils.Constants;

namespace mpost.SilverlightMultiFileUpload.Contracts
{
    public interface IUserFile
    {
        string FileName { get; set; }
        Enums.FileStates State { get; set; }
        string StateString { get; }
       
        double FileSize { get; }
        Stream FileStream { get; set; }

        double BytesUploaded { get; set; }
        double BytesUploadedFinished { get; set; }

        float Percentage { get; set; }
        float PercentageFinished { get; set; }

        string ErrorMessage { get; set; }

        void Upload(string initParams, Dispatcher uiDispatcher);
        void CancelUpload();

        event PropertyChangedEventHandler PropertyChanged;
    }
}