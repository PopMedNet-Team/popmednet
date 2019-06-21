using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.DataMartClient
{
    [DataContract]
    public class RequestListRow
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public long Identifier { get; set; }
        [DataMember]
        public string MSRequestID { get; set; }
        [DataMember]
        public bool AllowUnattendedProcessing { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string RequestTypePackageIdentifier { get; set; }
        [DataMember]
        public string AdapterPackageVersion { get; set; }
        [DataMember]
        public Enums.Priorities Priority { get; set; }
        [DataMember]
        public DateTime? DueDate { get; set; }
        [IgnoreDataMember]
        public string DueDateNoTime
        {
            get
            {
                if (DueDate.HasValue)
                    return DueDate.Value.ToShortDateString();

                return DueDate.ToString();
            }
        }
        [DataMember]
        public Enums.DMCRoutingStatus Status { get; set; }

        [IgnoreDataMember]
        public int RoutingStatus { get; set; }

        [DataMember]
        public DateTime Time { get; set; }
        [IgnoreDataMember]
        public DateTime TimeLocal
        {
            get
            {
                return Time.ToLocalTime();
            }
        }
        [DataMember]
        public string ProjectName { get; set; }

        [DataMember]
        public Guid DataMartID { get; set; }
        [DataMember]
        public string DataMartName { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public string RespondedBy { get; set; }
        [DataMember]
        public DateTime? ResponseTime { get; set; }
        [IgnoreDataMember]
        public DateTime? ResponseTimeLocal
        {
            get
            {
                if (ResponseTime.HasValue)
                    return ResponseTime.Value.ToLocalTime();

                return ResponseTime;
            }
        }
        [DataMember]
        public Guid? ResponseID { get; set; }
    }
}
