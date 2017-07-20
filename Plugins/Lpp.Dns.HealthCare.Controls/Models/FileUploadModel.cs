using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.Models
{

    public class FileSelection
    {
        public FileSelection() { }
        public FileSelection(String fileName, long size, Guid? docId = null, string dataMartName = null, string mimeType = null)
        {
            DataMartName = dataMartName;
            FileName = fileName;
            Size = size;
            MimeType = mimeType;
            ID = docId;
        }
        public string DataMartName { get; set; }
        public String FileName { get; set; }
        public string MimeType { get; set; }
        public long Size { get; set; }
        public Guid? ID { get; set; }
    }

    public class FileUploadModel
    {
        //public FileUploadModel()
        //{
        //    RequestFileList = new List<FileSelection>();
        //}
        public String UploadedFilesFieldName { get; set; }
        public String RemovedFilesFieldName { get; set; }
        public List<FileSelection> RequestFileList { get; set; }    // Files that have previously been uploaded for the request
        public String UploadedFileList { get; set; }                // Files that have been uploaded by the SL control since the last save
        public Guid RequestID { get; set; }                          // Id of the request hosting the control
        public string InitParams { get; set; }                      // SL Multifile Upload control settings
        public String RemovedFilesList { get; set; }                // Previously uploaded files the user has selected to remove from the request 
        public FileUploadDefinition Definition { get; set; }        // UIWidget definition
    }
}

