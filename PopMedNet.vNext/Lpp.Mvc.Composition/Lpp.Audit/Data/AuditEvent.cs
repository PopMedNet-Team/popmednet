using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Composition.Modules;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;

namespace Lpp.Audit.Data
{
    public class AuditEvent
    {
        public int Id { get; set; }
        public Security.Data.SecurityTargetId TargetId { get; set; }
        public Guid KindId { get; set; }
        public DateTime Time { get; set; }
        public virtual ICollection<AuditPropertyValue> PropertyValues { get; set; }

        public AuditEvent()
        {
            //TargetId = new Security.Data.SecurityTargetId();
            PropertyValues = new HashSet<AuditPropertyValue>();
        }
    }
}