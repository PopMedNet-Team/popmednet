using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using System.ComponentModel;

namespace Lpp.Dns.Portal.Models
{
    public class SharedFolderEditModel : ICrudSecObjectEditModel
    {
        public RequestSharedFolder Folder { get; set; }
        public string ReturnTo { get; set; }
        public bool AllowSave { get; set; }
        public bool AllowDelete { get; set; }
        public bool ShowAcl { get; set; }
    }
}