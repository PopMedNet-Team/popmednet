using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Model;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;

namespace Lpp.Dns.Portal.Models
{
    public class ObjectSelectorModel
    {
        public EntityForSelection? CurrentValue { get; set; }
        public EntitiesForSelectionModel SelectionList { get; set; }
        public string FieldName { get; set; }
        public bool AllowClear { get; set; }
        public string DialogTitle { get; set; }
    }
}