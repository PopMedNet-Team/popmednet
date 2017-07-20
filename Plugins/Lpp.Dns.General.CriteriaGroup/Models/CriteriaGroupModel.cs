using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Lpp.Dns.General.CriteriaGroup.Models
{
    public class CriteriaGroupModel
    {
        public string CriteriaGroupName { get; set; }
        public bool ExcludeCriteriaGroup { get; set; }
        public bool SaveCriteriaGroup { get; set; }

        [XmlIgnore]
        public int CriteriaGroupId { get; set; }

        [XmlIgnore]
        public TermSelectionsModel TermSelections { get; set; }

        [XmlIgnore]
        public bool Hidden { get; set; }

        public IEnumerable<TermModel> Terms { get; set; }
    }

    public class TermModel
    {
        public string TermName { get; set; }
        public IDictionary<string, string> Args
        {
            get
            {
                return args;
            }
        }

        private IDictionary<string, string> args = new Dictionary<string, string>();
    }
}
