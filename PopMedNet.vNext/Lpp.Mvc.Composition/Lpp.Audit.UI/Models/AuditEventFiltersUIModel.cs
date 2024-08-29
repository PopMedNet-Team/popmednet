using System;
using System.Collections.Generic;
using System.Linq;

namespace Lpp.Audit.UI.Models
{
    public class AuditEventFiltersUIModel
    {
        public string FieldName { get; set; }
        public ILookup<AuditEventKind, EventKindFiltersModel> InitialState { get; set; }
        public IDictionary<AuditEventKind, EventKindFiltersModel> DefaultFilterUIs { get; set; }
    }

    public class EventKindFiltersModel
    {
        public Guid FactoryId { get; set; }
        public ClientControlDisplay Display { get; set; }
    }
}