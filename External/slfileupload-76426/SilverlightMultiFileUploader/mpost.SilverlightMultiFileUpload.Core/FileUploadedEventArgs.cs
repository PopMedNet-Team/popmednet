using System;
using System.Windows.Browser;

/*
* Copyright Michiel Post
* http://www.michielpost.nl
* contact@michielpost.nl
*
* http://www.codeplex.com/SLFileUpload/
*
* */

namespace mpost.SilverlightMultiFileUpload.Core
{
    [ScriptableType]
    public class FileUploadedEventArgs : EventArgs
    {
        [ScriptableMember()]
        public string FileName { get; set; }
        public double FileSize { get; set; }

        public FileUploadedEventArgs(string fileName, double fileSize)
        {
            FileName = fileName;
            FileSize = fileSize;
        }
    }
}
