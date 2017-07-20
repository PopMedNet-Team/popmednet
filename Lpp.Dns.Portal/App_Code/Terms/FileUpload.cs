using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class FileUpload : IVisualTerm
    {
        public string Category
        {
            get
            {
                return null;
            }
        }

        public string CriteriaEditRelativePath
        {
            get
            {
                return "FileUpload/edit.cshtml";
            }
        }

        public string CriteriaViewRelativePath
        {
            get
            {
                return "FileUpload/view.cshtml";
            }
        }

        public string Description
        {
            get
            {
                return "Upload files for a file distribution request.";
            }
        }

        public string Name
        {
            get
            {
                return "File Upload";
            }
        }

        public string ProjectionEditRelativePath
        {
            get
            {
                return null;
            }
        }

        public string ProjectionViewRelativePath
        {
            get
            {
                return null;
            }
        }

        public string StratifierEditRelativePath
        {
            get
            {
                return null;
            }
        }

        public string StratifierViewRelativePath
        {
            get
            {
                return null;
            }
        }

        public Guid TermID
        {
            get
            {
                return ModelTermsFactory.FileUploadID;
            }
        }

        public object ValueTemplate
        {
            get
            {
                return new FileUploadValues();
            }
        }

        public class FileUploadValues
        {
            public IEnumerable<FileUploadDocumentModel> Documents { get; set; }
        }

        public class FileUploadDocumentModel
        {
            public Guid RevisionSetID { get; set; }
        }
    }
}