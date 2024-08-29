using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.QueryComposer
{
    [Serializable]
    public class ModelTerm : IModelTerm
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string OID { get; set; }

        public string ReferenceUrl { get; set; }
    }    
}
