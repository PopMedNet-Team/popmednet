using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Lpp.Dns.General.Metadata.Models
{
    [DataContract]
    public class MetadataSearchModel
    {
        public MetadataSearchModel()
        {
            //DxCodeSet = new HealthCare.Models.CodeSelectorModel();
            //PxCodeSet = new HealthCare.Models.CodeSelectorModel();
            //HCPCSCodeSet = new HealthCare.Models.CodeSelectorModel();
            //DrugClassCodeSet = new HealthCare.Models.CodeSelectorModel();
            //GenericCodeSet = new HealthCare.Models.CodeSelectorModel();
            Projects = new List<KeyValuePair<string, string>>();
            AllActivities = new List<TaskActivity>();
            WorkplanTypeList = new List<KeyValuePair<string, string>>();
            RequesterCenterList = new List<KeyValuePair<string, string>>();
            ReportAggregationLevelList = new List<KeyValuePair<string, string>>();
        }


        //This is very ugly.
        public List<KeyValuePair<string, string>> Projects { get; set; }
        public List<TaskActivity> AllActivities { get; set; }
        public List<KeyValuePair<string, string>> WorkplanTypeList { get; set; }
        public List<KeyValuePair<string, string>> RequesterCenterList { get; set; }
        public List<KeyValuePair<string, string>> ReportAggregationLevelList { get; set; }

        [DataMember]
        public string CriteriaGroupsJSON { get; set; }

        [DataMember]
        public string Report { get; set; }

        [DataMember]
        public Guid RequestId { get; set; }

        [DataMember]
        public Guid? TaskOrder { get; set; }

        [DataMember]
        public Guid? Activity { get; set; }

        [DataMember]
        public Guid? WorkplanTypeID { get; set; }

        [DataMember]
        public Guid? RequesterCenterID { get; set; }

        [DataMember]
        public Guid? ReportAggregationLevelID { get; set; }

        [DataMember]
        public Guid? ActivityProject { get; set; }

        [DataMember]
        public Guid? SourceTaskOrder { get; set; }

        [DataMember]
        public Guid? SourceActivity { get; set; }

        [DataMember]
        public Guid? SourceActivityProject { get; set; }

        [XmlIgnore]
        public string RequestName { get; set; }

        [XmlIgnore]
        public MetadataSearchRequestType RequestType { get; set; }

    }

    public class TaskActivity
    {
        public Guid ProjectID { get; set; }
        public string ActivityName { get; set; }
        public Guid ActivityID { get; set; }
        public Guid? ParentID { get; set; }
        public int TaskLevel { get; set; }
    }

}