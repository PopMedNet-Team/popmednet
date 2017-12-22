using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.General.CriteriaGroupCommon
{
    public static class RequestTypes
    {
        public static readonly Guid DATA_CHECKER_RACE = new Guid("{5CA5940A-CF8B-48CC-836C-66B2EB97AFB3}");
        public static readonly Guid DATA_CHECKER_ETHNICITY = new Guid("{4EE29758-DCFF-4D2A-A7A8-626C81FBA367}");
        public static readonly Guid DATA_CHECKER_DIAGNOSIS = new Guid("{D5DA7ACA-7179-4EA5-BD9C-534D47B6C6C4}");
        public static readonly Guid DATA_CHECKER_PROCEDURE = new Guid("{39F8E764-BDD8-4D75-AE50-809C59C28E43}");
        public static readonly Guid DATA_CHECKER_NDC = new Guid("{0F1EA011-B588-4775-9E16-CB6DBE12F8BE}");
        public static readonly Guid DATA_CHECKER_DIAGNOSIS_PDX = new Guid("{0F1EA012-B588-4775-9E16-CB6DBE12F8BE}");
        public static readonly Guid DATA_CHECKER_DISPENSING_RXAMT = new Guid("{0F1EA013-B588-4775-9E16-CB6DBE12F8BE}");
        public static readonly Guid DATA_CHECKER_DISPENSING_RXSUP = new Guid("{0F1EA014-B588-4775-9E16-CB6DBE12F8BE}");
        public static readonly Guid DATA_CHECKER_METADATA_COMPLETENESS = new Guid("{0F1EA015-B588-4775-9E16-CB6DBE12F8BE}");
    }
}
