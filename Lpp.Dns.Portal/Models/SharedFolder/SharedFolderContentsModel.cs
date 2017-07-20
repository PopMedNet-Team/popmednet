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
    public class SharedFolderContentsModel
    {
        public RequestListModel Requests { get; set; }
        public RequestSharedFolder Folder { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowRemove { get; set; }
    }
}