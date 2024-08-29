using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Lpp.Dns.General.Metadata.Models
{
    [DataContract]
    public class MetadataOrgSearchModel
    {
        [DataMember]
        public string Data { get; set; }

        [XmlIgnore]
        public string RequestName { get; set; }

        [XmlIgnore]
        public MetadataSearchRequestType RequestType { get; set; }
    }

    [DataContract]
    public class MetadataOrgSearchData {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string HealthPlanSystemDescription { get; set; }
        
        //Willing to Participate In
        [DataMember]
        public bool ObservationalResearch { get; set; }
        [DataMember]
        public bool PragamaticClincialTrials { get; set; }
        [DataMember]
        public bool ClinicalTrials { get; set; }

        //Data Model
        [DataMember]
        public bool DataModelMSCDM { get; set; }
        [DataMember]
        public bool DataModelESP { get; set; }
        [DataMember]
        public bool DataModelHMRON { get; set; }
        [DataMember]
        public bool DataModeli2b2 { get; set; }
        [DataMember]
        public bool DataModelOMOP { get; set; }
        [DataMember]
        public bool DataModelPCORI { get; set; }
        [DataMember]
        public bool DataModelOther { get; set; }

        //Types of Data Collected By Organization
        [DataMember]
        public bool None { get; set; }
        [DataMember]
        public bool Inpatient { get; set; }
        [DataMember]
        public bool Outpatient { get; set; }
        [DataMember]
        public bool PharmacyDispensings { get; set; }
        [DataMember]
        public bool Enrollment { get; set; }
        [DataMember]
        public bool Demographics { get; set; }
        [DataMember]
        public bool LaboratoryResults { get; set; }
        [DataMember]
        public bool VitalSigns { get; set; }
        [DataMember]
        public bool Biorepositories { get; set; }
        [DataMember]
        public bool PatientReportedOutcomes { get; set; }
        [DataMember]
        public bool PatientReportedBehaviors { get; set; }
        [DataMember]
        public bool PrescriptionOrders { get; set; }
        [DataMember]
        public bool DataCollectedOther { get; set; }
        
        //Electronic Health Records
        [DataMember]
        public bool InpatientNone  { get; set; }
        [DataMember]
        public bool InpatientEpic  { get; set; }
        [DataMember]
        public bool InpatientAllScripts  { get; set; }
        [DataMember]
        public bool InpatientEClinicalWorks  { get; set; }
        [DataMember]
        public bool InpatientNextGenHealthCare  { get; set; }
        [DataMember]
        public bool InpatientGEHealthCare  { get; set; }
        [DataMember]
        public bool InpatientMcKesson  { get; set; }
        [DataMember]
        public bool InpatientCare360  { get; set; }
        [DataMember]
        public bool InpatientCerner  { get; set; }
        [DataMember]
        public bool InpatientCPSI  { get; set; }
        [DataMember]
        public bool InpatientMeditech  { get; set; }
        [DataMember]
        public bool InpatientOther { get; set; }
        [DataMember]
        public bool InpatientVistA { get; set; }
        [DataMember]
        public bool InpatientSiemens { get; set; }
        [DataMember]
        public bool OutpatientNone { get; set; }
        [DataMember]
        public bool OutpatientEpic { get; set; }
        [DataMember]
        public bool OutpatientAllScripts { get; set; }
        [DataMember]
        public bool OutpatientEClinicalWorks { get; set; }
        [DataMember]
        public bool OutpatientNextGenHealthCare { get; set; }
        [DataMember]
        public bool OutpatientGEHealthCare { get; set; }
        [DataMember]
        public bool OutpatientMcKesson { get; set; }
        [DataMember]
        public bool OutpatientCare360 { get; set; }
        [DataMember]
        public bool OutpatientCerner { get; set; }
        [DataMember]
        public bool OutpatientCPSI { get; set; }
        [DataMember]
        public bool OutpatientMeditech { get; set; }
        [DataMember]
        public bool OutpatientOther { get; set; }
        [DataMember]
        public bool OutpatientSiemens { get; set; }
        [DataMember]
        public bool OutpatientVistA { get; set; }

        // Modular Program
        //[DataMember]
        //public bool MSReqID { get; set; }

        //[DataMember]
        //public bool MSProjID { get; set; }

        //[DataMember]
        //public bool MSWPType { get; set; }

        //[DataMember]
        //public bool MSWPID { get; set; }

        //[DataMember]
        //public bool MSVerID { get; set; }

        //[DataMember]
        //public bool RequestID { get; set; }

        //[DataMember]
        //public bool MP1Cycles { get; set; }

        //[DataMember]
        //public bool MP2Cycles { get; set; }

        //[DataMember]
        //public bool MP3Cycles { get; set; }

        //[DataMember]
        //public bool MP4Cycles { get; set; }

        //[DataMember]
        //public bool MP5Cycles { get; set; }

        //[DataMember]
        //public bool MP6Cycles { get; set; }

        //[DataMember]
        //public bool MP1Scenarios { get; set; }

        //[DataMember]
        //public bool MP2Scenarios { get; set; }

        //[DataMember]
        //public bool MP3Scenarios { get; set; }

        //[DataMember]
        //public bool MP4Scenarios { get; set; }

        //[DataMember]
        //public bool MP5Scenarios { get; set; }

        //[DataMember]
        //public bool MP6Scenarios { get; set; }

        //[DataMember]
        //public bool NumScen { get; set; }

        //[DataMember]
        //public bool NumCycle { get; set; }
    }
}
