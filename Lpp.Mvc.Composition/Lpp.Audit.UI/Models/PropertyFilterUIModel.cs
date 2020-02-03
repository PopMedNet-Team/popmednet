using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lpp.Composition.Modules;
using System.ComponentModel.Composition.Primitives;
using System.Web;
using System.Linq;
using System.Web.Mvc;

namespace Lpp.Audit.UI.Models
{
    public class PropertyFilterUIModel
    {
        public string OnChangeFunction { get; set; }
        public IEnumerable<PropertyFilterComponentUIModel> Properties { get; set; }
        public IEnumerable<IAuditProperty> AllPossibleProperties { get; set; }
    }

    public class PropertyFilterComponentUIModel
    {
        public IAuditProperty Property { get; set; }
        public PropertyValueComparison Comparison { get; set; }
        public ClientControlDisplay ValueSelector { get; set; }
    }

    public class ValueSelectorModel
    {
        public ClientControlDisplay Selector { get; set; }
        public string OnChangeFunction { get; set; }
    }
}