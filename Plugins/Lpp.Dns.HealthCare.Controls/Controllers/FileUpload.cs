using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc;
using Lpp.Dns.HealthCare.Controllers;
using Lpp.Dns.HealthCare.Models;

namespace Lpp.Dns.HealthCare
{
    [Export(typeof(IUIWidgetFactory<FileUpload>))]
    class FileUploadFactory : IUIWidgetFactory<FileUpload>
    {
        public FileUpload CreateWidget(HtmlHelper html)
        {
            return new FileUpload(html);
        }
    }

    public class FileUpload : IHtmlString, IUIWidget
    {
        public HtmlHelper Html { get; private set; }
        private readonly FileUploadDefinition _definition;

        public FileUpload(HtmlHelper html, FileUploadDefinition def = default( FileUploadDefinition ))
        {
            //Contract.Requires(html != null);
            Html = html;
            _definition = def;
        }

        public FileUpload AsPopup()
        {
            var d = _definition;
            d.AsPopup = true;
            return new FileUpload(Html, d);
        }

        public IHtmlString With(Guid requestID, List<FileSelection> requestFileList, string uploadedFilesFieldName, string removedFilesFieldName)
        {
            //Contract.Requires(!String.IsNullOrEmpty(uploadedFilesFieldName));
            //Contract.Requires(!String.IsNullOrEmpty(removedFilesFieldName));
            FileUploadModel model = new FileUploadModel
            {
                RequestID = requestID,
                InitParams = ",MaxFileSizeKB=2000000,MaxUploads=5,FileFilter=,ChunkSize=4194304,DefaultColor=#E3E6EB,CustomParams=" + requestID,
                RequestFileList = requestFileList,
                UploadedFilesFieldName = uploadedFilesFieldName,
                RemovedFilesFieldName = removedFilesFieldName
            };
            return Html.Partial<Views.FileUpload.MultifileUploader>().WithModel(model);
        }

        public IHtmlString With(Guid requestID, List<FileSelection> requestFileList)
        {
            FileUploadModel model = new FileUploadModel
            {
                RequestID = requestID,
                RequestFileList = requestFileList,
            };
           return Html.Partial<Views.FileUpload.FileUploadDisplay>().WithModel(model);
        }
        
        public string ToHtmlString()
        {
            Html.RenderAction<FileUploadController>(c => c.MultifileUploader(_definition));
            return "";
        }
        
    }

    public struct FileUploadDefinition
    {
        public bool AsPopup { get; set; }
    }
}