using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Lpp.Dns.General.CriteriaGroup.Models
{
    public class TermSelectionsModel
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public IEnumerable<TermSelectionModel> Terms { get; set; }
    }

    public class TermSelectionModel
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public IEnumerable<TermSelectionModel> Terms { get; set; }
    }
}
