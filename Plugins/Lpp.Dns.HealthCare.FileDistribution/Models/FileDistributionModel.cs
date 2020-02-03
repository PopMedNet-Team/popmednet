using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.HealthCare.Models;

namespace Lpp.Dns.HealthCare.FileDistribution.Models
{

    public class FileDistributionModel
    {
        public FileDistributionModel()
        {
            RequestFileList = new List<FileSelection>();
        }
        public FileDistributionRequestType RequestType { get; set; }
        public List<FileSelection> RequestFileList { get; set; }    // Files that have previously been uploaded for the request
        public Guid RequestId { get; set; }
        public string InitParams { get; set; }
        public String UploadedFileList { get; set; }
        public String RemovedFilesList { get; set; }

    }
}

