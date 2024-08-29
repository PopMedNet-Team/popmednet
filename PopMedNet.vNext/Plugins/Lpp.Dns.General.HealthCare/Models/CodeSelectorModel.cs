using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;

namespace Lpp.Dns.General.HealthCare.Models
{
    public class CodeSelectorModel
    {
        public IEnumerable<LookupListCategory> Categories { get; set; }
        public CodeSelectorDefinition Definition { get; set; }
        public IEnumerable<LookupListValue> SelectedCodes { get; set; }
    }
}
