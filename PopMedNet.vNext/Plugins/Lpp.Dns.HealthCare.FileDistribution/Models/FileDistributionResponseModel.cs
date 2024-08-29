using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.HealthCare.Models;
using Lpp.Dns.Data;

namespace Lpp.Dns.HealthCare.FileDistribution.Models
{

    public class FileDistributionResponseModel
    {
        public FileDistributionResponseModel()
        {
            ResponseFileList = new List<FileDistributionResponse>();
            
        }
        public List<FileDistributionResponse> ResponseFileList { get; set; }

    }

    public class FileDistributionResponse
    {
        public string DataMartName { get; set; }
        public Document ResponseFile { get; set; }
        public FileDistributionResponse(string datamartName, Document doc)
        {
            DataMartName = datamartName;
            //If we pass documents with ParentDocumentID set for  re-uploaded documents, a self-reference error is thrown.
            //Adding ReferenceLoopHandling.Ignore or Serialize doesn't fix/resolve it either.
            //ResponseFile = doc;

            //So we create a copy of the document and use that
            ResponseFile = new Document();
            ResponseFile.BuildVersion = doc.BuildVersion;
            ResponseFile.CreatedOn = doc.CreatedOn;
            ResponseFile.Description = doc.Description;
            ResponseFile.DocumentChangeLogs = doc.DocumentChangeLogs;
            ResponseFile.FileName = doc.FileName;
            ResponseFile.ID = doc.ID;
            ResponseFile.ItemID = doc.ItemID;
            ResponseFile.Kind = doc.Kind;
            ResponseFile.Length = doc.Length;
            ResponseFile.MajorVersion = doc.MajorVersion;
            ResponseFile.MimeType = doc.MimeType;
            ResponseFile.MinorVersion = doc.MinorVersion;
            ResponseFile.Name = doc.Name;
            ResponseFile.RevisionDescription = doc.RevisionDescription;
            ResponseFile.RevisionSetID = doc.RevisionSetID;
            ResponseFile.RevisionVersion = doc.RevisionVersion;
            ResponseFile.UploadedBy = doc.UploadedBy;
            ResponseFile.UploadedByID = doc.UploadedByID;
            ResponseFile.Viewable = doc.Viewable;
        }
    }
}

