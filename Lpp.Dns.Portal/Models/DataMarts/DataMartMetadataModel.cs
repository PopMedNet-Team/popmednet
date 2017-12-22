using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using Lpp.Security.UI;
using Lpp.Security;
using System.Data;

namespace Lpp.Dns.Portal.Models
{
    public class DataMartMetadataModel
    {
        public string DataMartName { get; set; }
        public Guid DataMartID { get; set; }
        public IList<ModelMetadata> ModelMetadataList { get; set; }
        public bool ShowDocuments { get { return false; } }
        public string ReturnTo { get; set; }
    }

    public class ModelMetadata
    {
        public string ModelName { get; set; }
        public Guid ModelID { get; set; }
        public IList<RequestTypeResponse> Responses { get; set; }
    }

    public class RequestTypeResponse
    {
        public Guid RequestId { get; set; }
        public Guid RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
        public Guid ResponseId { get; set; }
        public Func<HtmlHelper, IHtmlString> BodyView { get; set; }
    }
}