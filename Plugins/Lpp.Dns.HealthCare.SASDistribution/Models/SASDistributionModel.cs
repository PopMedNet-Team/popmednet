using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.HealthCare.Models;

namespace Lpp.Dns.HealthCare.SASDistribution.Models
{

    public class SASDistributionModel
    {
        public SASDistributionModel()
        {
            RequestFileList = new List<FileSelection>();
        }
        public SASDistributionRequestType RequestType { get; set; }
        public List<FileSelection> RequestFileList { get; set; }    // Files that have previously been uploaded for the request
        public Guid RequestID { get; set; }
        public string InitParams { get; set; }
        public String UploadedFileList { get; set; }
        public String RemovedFilesList { get; set; }

    }
}

